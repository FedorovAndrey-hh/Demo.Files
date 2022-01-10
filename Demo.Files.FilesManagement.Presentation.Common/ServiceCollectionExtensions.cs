using Common.Communication.RabbitMq.DependencyInjection;
using Common.Core.Diagnostics;
using Common.Core.Events;
using Common.Core.Work;
using Common.DependencyInjection;
using Common.EntityFramework.Postgres;
using Common.Persistence.EntityFramework;
using Demo.Files.Common.Configuration;
using Demo.Files.Common.Contracts.Communication;
using Demo.Files.FilesManagement.Applications.Abstractions;
using Demo.Files.FilesManagement.Applications.Implementations.ByDomain;
using Demo.Files.FilesManagement.Domain.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.Postgres;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Files.FilesManagement.Presentation.Common;

public static class UseCasesServiceCollectionExtensions
{
	public static void AddUseCasesWithDomainImplementation(
		this IServiceCollection @this,
		IConfiguration persistenceConfiguration,
		IConfiguration communicationConfiguration)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(persistenceConfiguration, nameof(persistenceConfiguration));
		Preconditions.RequiresNotNull(communicationConfiguration, nameof(communicationConfiguration));

		@this.Configure<FilesManagementPersistenceOptions>(persistenceConfiguration);
		@this.AddScoped<FilesManagementDbContext>(
			services =>
			{
				var options = services.GetRequiredOptions<FilesManagementPersistenceOptions>();

				return new PostgresFilesManagementDbContext(
					new PostgresConnectionInfo(
						options.Server
						?? throw Errors.EnvironmentVariableNotFound(nameof(PostgresConnectionInfo.Server)),
						options.Port
						?? throw Errors.EnvironmentVariableNotFound(nameof(PostgresConnectionInfo.Port)),
						options.Database
						?? throw Errors.EnvironmentVariableNotFound(nameof(PostgresConnectionInfo.Database)),
						options.Username
						?? throw Errors.EnvironmentVariableNotFound(nameof(PostgresConnectionInfo.Username)),
						options.Password
						?? throw Errors.EnvironmentVariableNotFound(nameof(PostgresConnectionInfo.Password))
					)
				);
			}
		);
		@this.AddDatabaseEnvironmentStateDetector<FilesManagementDbContext>(TimeSpan.FromSeconds(1));

		@this.AddScoped<StorageReadContext>();
		@this.AddScopedBinding<Storage.IReadContext, StorageReadContext>();
		@this.AddScoped<IFilesManagementReadContext, FilesManagementReadContext>();
		@this.AddScopedBinding<IFilesManagementPersistenceContext, FilesManagementDbContext>();

		@this.AddScoped<IAsyncWorkScopeProvider<IFilesManagementWriteContext>, FilesManagementWorkScopeProvider>();

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
			IAsyncEventPublisher<StorageEvent.Created>,
			IInternalAsyncEventPublisher>();
		@this.AddScoped<ICreatePremiumStorage, CreatePremiumStorage>();

		@this.AddScopedBinding<
			IAsyncEventPublisher<StorageEvent.Modified.DirectoryAdded>,
			IInternalAsyncEventPublisher>();
		@this.AddScoped<IAddDirectory, AddDirectory>();

		@this.AddScopedBinding<
			IAsyncEventPublisher<StorageEvent.Modified.DirectoryRelocated>,
			IInternalAsyncEventPublisher>();
		@this.AddScoped<IRelocateDirectory, RelocateDirectory>();

		@this.AddScopedBinding<
			IAsyncEventPublisher<StorageEvent.Modified.DirectoryRemoved>,
			IInternalAsyncEventPublisher>();
		@this.AddScoped<IRemoveDirectory, RemoveDirectory>();

		@this.AddScopedBinding<
			IAsyncEventPublisher<StorageEvent.Modified.DirectoryRenamed>,
			IInternalAsyncEventPublisher>();
		@this.AddScoped<IRenameDirectory, RenameDirectory>();
	}
}