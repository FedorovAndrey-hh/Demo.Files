using Common.Core.Diagnostics;
using Common.DependencyInjection;
using Common.Persistence;
using Common.Persistence.EntityFramework;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility;

public sealed class DebugPlugin : IServicesPlugin
{
	public static async Task HandleDebugOptionsAsync(IServiceProvider services)
	{
		Preconditions.RequiresNotNull(services, nameof(services));

		var programOptions = services.GetRequiredService<IConfiguration>()
			.GetSection(nameof(ProgramDebugOptions))
			.Get<ProgramDebugOptions>();

		if (programOptions.WipePersistenceOnStart)
		{
			await using (var scope = services.CreateAsyncScope())
			{
				await scope.ServiceProvider.WipeAllPersistenceAsync().ConfigureAwait(false);
			}
		}

		if (programOptions.MigratePersistenceOnStart)
		{
			await using (var scope = services.CreateAsyncScope())
			{
				await scope.ServiceProvider.InitializeAllPersistenceAsync().ConfigureAwait(false);
			}
		}
	}

	public void InstallTo(IServiceCollection services)
	{
		Preconditions.RequiresNotNull(services, nameof(services));

		services.AddScoped<IPersistenceInitializer, Initializer>();
		services.AddScoped<IPersistenceWiper, Wiper>();
	}

	private sealed class Initializer : IPersistenceInitializer
	{
		public Initializer(AuthorizationDbContext dbContext)
		{
			Preconditions.RequiresNotNull(dbContext, nameof(dbContext));

			_dbContext = dbContext;
		}

		private readonly AuthorizationDbContext _dbContext;

		public Task InitializeAsync() => _dbContext.Database.MigrateAsync();
	}

	private sealed class Wiper : IPersistenceWiper
	{
		public Wiper(AuthorizationDbContext dbContext)
		{
			Preconditions.RequiresNotNull(dbContext, nameof(dbContext));

			_dbContext = dbContext;
		}

		private readonly AuthorizationDbContext _dbContext;

		public Task WiperAsync() => _dbContext.Database.EnsureDeletedAsync();
	}
}