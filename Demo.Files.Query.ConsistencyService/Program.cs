using Common.Communication;
using Common.Communication.DependencyInjection;
using Common.Communication.RabbitMq.DependencyInjection;
using Common.Core;
using Common.Core.Diagnostics;
using Common.DependencyInjection;
using Demo.Files.Common.Configuration;
using Demo.Files.Common.Contracts.Communication;
using Demo.Files.Common.Contracts.Communication.FilesManagement;
using Demo.Files.Query.Views.Directories;
using Demo.Files.Query.Views.MongoDb;
using Demo.Files.Query.Views.Storages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Files.Query.ConsistencyService;

internal static class Program
{
	internal static async Task Main(params string[] args)
	{
		Preconditions.RequiresNotNull(args, nameof(args));

		using (var services = _BuilderServices(args))
		{
			await services.WaitForEnvironmentSetupAsync(Programs.CancelOrProcessExitCancellationToken())
				.ConfigureAwait(false);

			var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(Program));

			logger.LogInformation("Application started");

			await using (var scope = services.CreateAsyncScope())
			{
				var scopedServices = scope.ServiceProvider;
				_HandleMessages(
					scopedServices,
					scopedServices.GetRequiredService<IMessageReceiver>()
				);

				logger.LogInformation("Waiting for events");

				await Programs.WaitForCancelOrProcessExitAsync().ConfigureAwait(false);
			}
		}
	}

	private static ServiceProvider _BuilderServices(string[] args)
	{
		Preconditions.RequiresNotNull(args, nameof(args));

		var configuration = ProgramConfiguration.Create(typeof(Program).Assembly, args);

		var servicesBuilder = new ServiceCollection();
		servicesBuilder.AddSingleton<IConfiguration>(configuration.App);

		var environment = configuration.Host.GetValue<string>(ProgramConfiguration.EnvironmentKey);

		servicesBuilder.AddLogging(
			builder =>
			{
				ProgramConfiguration.ConfigureLogging(
					builder,
					configuration.App,
					environment
				);
			}
		);

		servicesBuilder.AddRabbitMqCommunication(
			ServiceLifetime.Singleton,
			configuration.App.GetSection("CommunicationOptions"),
			options =>
			{
				options.Persistent = true;
				var jsonSerializerOptions = SerializationConfiguration.ConfigureJson();
				jsonSerializerOptions.AddFilesCommunicationAdapters();
				options.Json = jsonSerializerOptions;
			}
		);

		servicesBuilder.AddScopedBinding<
			IMessageHandler<StorageCreatedMessage>,
			IStorageCompactViewsConsistency>();
		servicesBuilder.AddScopedBinding<
			IMessageHandler<StorageDirectoryAddedMessage>,
			IStorageCompactViewsConsistency>();

		servicesBuilder.AddScopedBinding<
			IMessageHandler<StorageDirectoryAddedMessage>,
			IDirectoryCompactViewsConsistency>();

		servicesBuilder.AddMongoDbFilesViews(
			configuration.App.GetSection("PersistenceOptions"),
			options => options.Json = SerializationConfiguration.ConfigureJson()
		);

		return servicesBuilder
			.BuildServiceProvider(ProgramConfiguration.ConfigureServiceProvider(environment));
	}

	private static void _HandleMessages(IServiceProvider services, IMessageReceiver messageReceiver)
	{
		Preconditions.RequiresNotNull(services, nameof(services));
		Preconditions.RequiresNotNull(messageReceiver, nameof(messageReceiver));

		messageReceiver.OnReceiveMessage<StorageCreatedMessage>(
			StorageCreatedMessage.Port,
			new ScopedMessageHandler<StorageCreatedMessage>(services, allowMultiple: true)
		);

		messageReceiver.OnReceiveMessage<StorageDirectoryAddedMessage>(
			StorageDirectoryAddedMessage.Port,
			new ScopedMessageHandler<StorageDirectoryAddedMessage>(services, allowMultiple: true)
		);
	}
}