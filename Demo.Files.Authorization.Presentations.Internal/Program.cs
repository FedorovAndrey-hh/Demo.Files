using Common.Communication;
using Common.Communication.DependencyInjection;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Execution.Retry;
using Common.DependencyInjection;
using Demo.Files.Authorization.Presentation.Common;
using Demo.Files.Common.Configuration;
using Demo.Files.Common.Contracts.Communication.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Files.Authorization.Presentations.Internal;

internal static class Program
{
	internal static async Task Main(params string[] args)
	{
		using (var services = _BuildServices(args))
		{
			await services.WaitForEnvironmentSetupAsync(Programs.CancelOrProcessExitCancellationToken())
				.ConfigureAwait(false);

			var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(Program));

			logger.LogInformation("Application started");

			using (var scope = services.CreateScope())
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

	private static ServiceProvider _BuildServices(string[] args)
	{
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

		servicesBuilder.AddUseCasesWithDomainImplementation(
			configuration.App.GetSection("PersistenceOptions"),
			configuration.App.GetSection("CommunicationOptions")
		);

		servicesBuilder.AddScoped<
			IMessageHandler<PremiumStorageCreatedMessage>,
			PremiumStorageCreatedMessageHandler>();

		servicesBuilder.AddSingleton<IDelayedRetryPolicy, MessageHandlerRetryPolicy>();

		return servicesBuilder.BuildServiceProvider(ProgramConfiguration.ConfigureServiceProvider(environment));
	}

	private static void _HandleMessages(IServiceProvider services, IMessageReceiver messageReceiver)
	{
		Preconditions.RequiresNotNull(services, nameof(services));
		Preconditions.RequiresNotNull(messageReceiver, nameof(messageReceiver));

		messageReceiver.OnReceiveMessage<PremiumStorageCreatedMessage>(
			PremiumStorageCreatedMessage.Port,
			new ScopedMessageHandler<PremiumStorageCreatedMessage>(services, allowMultiple: false)
		);
	}
}