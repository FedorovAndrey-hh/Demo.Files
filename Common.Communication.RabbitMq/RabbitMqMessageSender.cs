using System.Text;
using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common.Communication.RabbitMq;

public sealed class RabbitMqMessageSender : IMessageSender
{
	public RabbitMqMessageSender(
		IConnection connection,
		IMessageSerializer serializer,
		bool persistent,
		ILogger<RabbitMqMessageSender>? logger)
	{
		Preconditions.RequiresNotNull(connection, nameof(connection));
		Preconditions.RequiresNotNull(serializer, nameof(serializer));

		_connection = connection;
		_serializer = serializer;
		_persistent = persistent;
		_logger = logger;
	}

	private readonly IConnection _connection;
	private object _modelLock = new();
	private IModel? _model;

	private IModel Model
	{
		get
		{
			lock (_modelLock)
			{
				return _model ?? (_model = _connection.CreateModel());
			}
		}
	}

	private readonly IMessageSerializer _serializer;
	private readonly bool _persistent;
	private readonly ILogger<RabbitMqMessageSender>? _logger;

	public Task SendMessageAsync(string port, object message)
	{
		Preconditions.RequiresNotNull(port, nameof(port));
		Preconditions.RequiresNotNull(message, nameof(message));

		var model = Model;

		lock (_modelLock)
		{
			model.QueueDeclare(
				queue: port,
				durable: _persistent,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);
		}

		IBasicProperties? properties;
		if (_persistent)
		{
			lock (_modelLock)
			{
				properties = model.CreateBasicProperties();
				properties.Persistent = true;
			}
		}
		else
		{
			properties = null;
		}

		var body = _serializer.SerializeMessage(message);

		if (_logger is not null && _logger.IsEnabled(LogLevel.Debug))
		{
			_logger.LogDebug("Publish message: `{Message}` from `{Port}`", Encoding.UTF8.GetString(body), port);
		}

		lock (_modelLock)
		{
			model.BasicPublish(
				exchange: "",
				routingKey: port,
				basicProperties: properties,
				body: body
			);
		}

		return Task.CompletedTask;
	}
}