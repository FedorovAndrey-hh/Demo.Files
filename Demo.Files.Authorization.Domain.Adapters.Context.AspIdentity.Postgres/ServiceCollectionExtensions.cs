using Common.Core.Diagnostics;
using Common.DependencyInjection;
using Common.EntityFramework.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres;

public static class ServiceCollectionExtensions
{
	public static void AddPostgresAspIdentityAuthorization(
		this IServiceCollection @this,
		IConfiguration? configuration = null,
		Action<PostgresAspIdentityAuthorizationOptions>? configure = null)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (configuration is not null)
		{
			@this.Configure<PostgresAspIdentityAuthorizationOptions>(configuration);
		}

		if (configure is not null)
		{
			@this.PostConfigure<PostgresAspIdentityAuthorizationOptions>(configure);
		}

		@this
			.AddScoped<PostgresAuthorizationDbContext>(
				services =>
				{
					var options = services.GetRequiredOptions<PostgresAspIdentityAuthorizationOptions>();

					return new PostgresAuthorizationDbContext(
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
			)
			.AddAspIdentityAuthorization<PostgresAuthorizationDbContext>();
	}
}