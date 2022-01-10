using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Environments;
using Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common.Communication.RabbitMq.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static void AddRabbitMqCommunication(
		this IServiceCollection @this,
		ServiceLifetime connectionLifetime = ServiceLifetime.Scoped,
		IConfiguration? configuration = null,
		Action<RabbitMqOptions>? configure = null)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (configuration is not null)
		{
			@this.Configure<RabbitMqOptions>(configuration);
		}

		if (configure is not null)
		{
			@this.PostConfigure(configure);
		}

		@this.AddSingleton<IMessageSerializer>(
			services => new JsonMessageSerializer(services.GetOptions<RabbitMqOptions>()?.Json)
		);

		@this.AddScoped<IMessageReceiver>(
			services =>
			{
				var options = services.GetOptions<RabbitMqOptions>();

				return new RabbitMqMessageReceiver(
					services.CreateRequiredTaggedService<IConnection, ReceiverTag>(),
					services.GetRequiredService<IMessageSerializer>(),
					options?.Persistent ?? false,
					options?.ResendOnFailedHandle ?? false,
					services.GetService<ILogger<RabbitMqMessageReceiver>>()
				);
			}
		);

		@this.AddScoped<IMessageSender>(
			services =>
			{
				var options = services.GetOptions<RabbitMqOptions>();

				return new RabbitMqMessageSender(
					services.CreateRequiredTaggedService<IConnection, SenderTag>(),
					services.GetRequiredService<IMessageSerializer>(),
					options?.Persistent ?? false,
					services.GetService<ILogger<RabbitMqMessageSender>>()
				);
			}
		);

		@this.Add(
			new ServiceDescriptor(
				typeof(ITaggedFactory<IConnection, ReceiverTag>),
				services => new TaggedInstanceFactory<IConnection, ReceiverTag>(
					services.GetRequiredService<IAsyncConnectionFactory>().CreateConnection()
				),
				connectionLifetime
			)
		);
		@this.Add(
			new ServiceDescriptor(
				typeof(ITaggedFactory<IConnection, SenderTag>),
				services => new TaggedInstanceFactory<IConnection, SenderTag>(
					services.GetRequiredService<IAsyncConnectionFactory>().CreateConnection()
				),
				connectionLifetime
			)
		);

		@this.AddSingleton<IEnvironmentStateDetector>(
			services =>
			{
				var options = services.GetOptions<RabbitMqOptions>();

				return new RabbitMqEnvironmentStateDetector(
					MathExtensions.Max(TimeSpan.FromSeconds(1), options?.ReadyCheckPeriod ?? TimeSpan.Zero),
					services.GetRequiredService<IConnectionFactory>()
				);
			}
		);

		@this.AddSingleton<IConnectionFactory>(
			services =>
			{
				var options = services.GetOptions<RabbitMqOptions>();

				var connectionFactory = new ConnectionFactory();

				ApplyOptions(connectionFactory, options);

				return connectionFactory;
			}
		);

		@this.AddSingleton<IAsyncConnectionFactory>(
			services =>
			{
				var options = services.GetOptions<RabbitMqOptions>();

				var connectionFactory = new ConnectionFactory();

				ApplyOptions(connectionFactory, options);

				connectionFactory.DispatchConsumersAsync = true;

				return connectionFactory;
			}
		);
	}

	private static void ApplyOptions(ConnectionFactory connectionFactory, RabbitMqOptions? options)
	{
		Preconditions.RequiresNotNull(connectionFactory, nameof(connectionFactory));

		if (options is not null)
		{
			var host = options.Host;
			if (host is not null)
			{
				connectionFactory.HostName = host;
			}

			var port = options.Port;
			if (port.HasValue)
			{
				connectionFactory.Port = port.Value;
			}

			var username = options.Username;
			if (username is not null)
			{
				connectionFactory.UserName = username;
			}

			var password = options.Password;
			if (username is not null)
			{
				connectionFactory.Password = password;
			}
		}
	}
}