using Common.EntityFramework.Postgres;
using Microsoft.EntityFrameworkCore.Design;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.Postgres;

public sealed class PostgresFilesManagementDesignTimeDbContextFactory
	: IDesignTimeDbContextFactory<PostgresFilesManagementDbContext>
{
	public PostgresFilesManagementDbContext CreateDbContext(string[] args)
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