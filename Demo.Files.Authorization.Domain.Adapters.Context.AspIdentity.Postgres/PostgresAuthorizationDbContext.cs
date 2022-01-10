using Common.Core.Diagnostics;
using Common.EntityFramework.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres;

public sealed class PostgresAuthorizationDbContext : AuthorizationDbContext
{
	public PostgresAuthorizationDbContext(PostgresConnectionInfo connectionInfo)
	{
		Preconditions.RequiresNotNull(connectionInfo, nameof(connectionInfo));

		_connectionInfo = connectionInfo;
	}

	private readonly PostgresConnectionInfo _connectionInfo;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		Preconditions.RequiresNotNull(optionsBuilder, nameof(optionsBuilder));
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseNpgsql(_connectionInfo.GetConnectionString());
	}
}