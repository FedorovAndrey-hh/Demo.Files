using Microsoft.EntityFrameworkCore.Design;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.LocalDb;

public sealed class LocalDbFilesManagementDesignTimeDbContextFactory
	: IDesignTimeDbContextFactory<LocalDbFilesManagementDbContext>
{
	public LocalDbFilesManagementDbContext CreateDbContext(string[] args) => new("design-time");
}