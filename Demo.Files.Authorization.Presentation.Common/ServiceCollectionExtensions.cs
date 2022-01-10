using Common.Communication.RabbitMq.DependencyInjection;
using Common.Core.Diagnostics;
using Common.Core.Events;
using Common.DependencyInjection;
using Common.Persistence.EntityFramework;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Applications.Implementations.ByDomain;
using Demo.Files.Authorization.Applications.Implementations.ByDomain.AspIdentity;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres;
using Demo.Files.Common.Configuration;
using Demo.Files.Common.Contracts.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Files.Authorization.Presentation.Common;

public static class ServiceCollectionExtensions
{
	public static void AddUseCasesWithDomainImplementation(
		this IServiceCollection @this,
		IConfiguration persistenceConfiguration,
		IConfiguration communicationConfiguration)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(persistenceConfiguration, nameof(persistenceConfiguration));
		Preconditions.RequiresNotNull(communicationConfiguration, nameof(communicationConfiguration));

		@this.AddPostgresAspIdentityAuthorization(persistenceConfiguration);
		@this.AddDatabaseEnvironmentStateDetector<PostgresAuthorizationDbContext>(TimeSpan.FromSeconds(1));

		@this.AddRabbitMqCommunication(
			ServiceLifetime.Singleton,
			communicationConfiguration,
			options =>
			{
				options.Persistent = true;
				var jsonSerializerOptions = SerializationConfiguration.ConfigureJson();
				jsonSerializerOptions.AddFilesCommunicationAdapters();
				options.Json = jsonSerializerOptions;
			}
		);

		@this.AddScoped<IInternalAsyncEventPublisher, InternalCommunicationEventPublisher>();
		@this.AddScopedBinding<
			IAsyncEventPublisher<UserEvent.Modified.ResourceRequested>,
			IInternalAsyncEventPublisher>();

		@this.AddScoped<IAsGuestRegister, AsGuestRegister>();
		@this.AddScoped<IAsGuestSignIn, AsGuestSignIn>();
		@this.AddScoped<IAsUserAcquireStorageResource, AsUserAcquireStorageResource>();
		@this.AddScoped<IAsUserChangePassword, AsUserChangePassword>();
		@this.AddScoped<IAsUserChangeUsername, AsUserChangeUsername>();
		@this.AddScoped<IAsUserGetRequestedStorageResource, AsUserGetRequestedStorageResource>();
		@this.AddScoped<IAsUserRequestStorageResource, AsUserRequestStorageResource>();
	}
}