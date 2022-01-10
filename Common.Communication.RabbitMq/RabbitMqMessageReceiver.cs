using System.Collections.Immutable;
using System.Text;
using Common.Core;
using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Communication.RabbitMq;

public sealed class RabbitMqMessageReceiver
	: IMessageReceiver,
	  IDisposable
{
	public RabbitMqMessageReceiver(
		IConnection connection,
		IMessageSerializer serializer,
		bool persistent,
		bool resendOnFail,
		ILogger<RabbitMqMessageReceiver>? logger)
	{
		Preconditions.RequiresNotNull(connection, nameof(connection));
		Preconditions.RequiresNotNull(serializer, nameof(serializer));

		_connection = connection;
		_serializer = serializer;
		_persistent = persistent;
		_resendOnFail = resendOnFail;
		_logger = logger;
	}

	private readonly IConnection _connection;
	private readonly object _lock = new();
	private volatile IModel? _model;

	[GuardedBy(nameof(_lock))]
	private IModel _Model => _model ?? (_model = _connection.CreateModel());

	private readonly IMessageSerializer _serializer;
	private readonly bool _persistent;
	private readonly bool _resendOnFail;
	private readonly ILogger<RabbitMqMessageReceiver>? _logger;

	private IImmutableSet<string> _tags = ImmutableHashSet.Create<string>();

	public void OnReceiveMessage<TMessage>(string port, IMessageHandler<TMessage> handler)
		where TMessage : notnull
	{
		Preconditions.RequiresNotNull(port, nameof(port));
		Preconditions.RequiresNotNull(handler, nameof(handler));

		lock (_lock)
		{
			_CheckDisposed();

			var model = _Model;

			model.QueueDeclare(
				queue: port,
				durable: _persistent,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);

			var consumer = new AsyncEventingBasicConsumer(model);

			consumer.Received += async (_, args) =>
			{
				try
				{
					if (_logger is not null && _logger.IsEnabled(LogLevel.Debug))
					{
						_logger.LogDebug(
							"Message received: `{Message}` from `{Port}`",
							Encoding.UTF8.GetString(args.Body.Span),
							port
						);
					}

					var result = await handler
						.HandleMessageAsync(_serializer.DeserializeMessage<TMessage>(args.Body.ToArray()))
						.ConfigureAwait(false);

					_Report(args.DeliveryTag, result);
				}
				catch (Exception e)
				{
					_logger?.LogCritical(e, "Fail to handle receive from `{Port}`", port);

					lock (_lock)
					{
						try
						{
							model.BasicNack(args.DeliveryTag, multiple: false, requeue: _resendOnFail);
						}
						catch (Exception exception)
						{
							_logger?.LogCritical(exception, "Fail to report failed receive from `{Port}`", port);
							throw;
						}
					}
				}
			};

			var tag = model.BasicConsume(consumer, port, autoAck: false);

			_tags = _tags.Add(tag);
		}
	}

	private void _Report(ulong deliveryTag, HandleMessageResult result)
	{
		switch (result)
		{
			case HandleMessageResult.Success:
				lock (_lock)
				{
					_Model.BasicAck(deliveryTag, multiple: false);
				}

				break;
			case HandleMessageResult.CurrentlyUnprocessable:
				lock (_lock)
				{
					_Model.BasicNack(deliveryTag, multiple: false, requeue: true);
				}

				break;
			case HandleMessageResult.Invalid:
				lock (_lock)
				{
					_Model.BasicNack(deliveryTag, multiple: false, requeue: false);
				}

				break;
			default:
				throw Errors.UnsupportedEnumValue(result);
		}
	}

	public void Dispose()
	{
		lock (_lock)
		{
			if (!_disposed)
			{
				_disposed = true;

				var tags = _tags;
				_tags = ImmutableHashSet.Create<string>();

				var model = _model;
				if (model is not null)
				{
					foreach (var tag in tags)
					{
						model.BasicCancel(tag);
					}
				}
			}
		}
	}

	private bool _disposed = false;

	private void _CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(nameof(RabbitMqMessageReceiver));
		}
	}
}