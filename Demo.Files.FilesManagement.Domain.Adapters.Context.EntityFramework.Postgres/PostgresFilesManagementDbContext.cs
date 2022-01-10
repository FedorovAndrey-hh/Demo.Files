using Common.Core.Diagnostics;
using Common.EntityFramework.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.Postgres;

public sealed class PostgresFilesManagementDbContext : FilesManagementDbContext
{
	public PostgresFilesManagementDbContext(PostgresConnectionInfo connectionInfo)
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