using Common.Core.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.LocalDb;

public sealed class LocalDbFilesManagementDbContext : FilesManagementDbContext
{
	public LocalDbFilesManagementDbContext(string database)
	{
		Preconditions.RequiresNotNull(database, nameof(database));

		_database = database;
	}

	private readonly string _database;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		Preconditions.RequiresNotNull(optionsBuilder, nameof(optionsBuilder));
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseSqlServer(@$"Server=(localdb)\mssqllocaldb;Database={_database}");
	}
}