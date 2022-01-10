using Common.EntityFramework.Postgres;
using Microsoft.EntityFrameworkCore.Design;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres;

public sealed class PostgresAspIdentityAuthorizationDesignTimeDbContextFactory
	: IDesignTimeDbContextFactory<PostgresAuthorizationDbContext>
{
	public PostgresAuthorizationDbContext CreateDbContext(string[] args)
		=> new(
			new PostgresConnectionInfo(
				"127.0.0.1",
				5432,
				"design-time",
				"design-time",
				"design-time"
			)
		);
}