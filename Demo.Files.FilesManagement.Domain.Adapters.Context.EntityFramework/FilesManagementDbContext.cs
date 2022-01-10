using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

public class FilesManagementDbContext
	: DbContext,
	  IFilesManagementPersistenceContext
{
	public FilesManagementDbContext()
	{
	}

	public FilesManagementDbContext(DbContextOptions options)
		: base(Preconditions.RequiresNotNull(options, nameof(options)))
	{
	}

	public DbSet<StorageData> Storages { get; internal set; } = null!;
	public DbSet<DirectoryData> Directories { get; internal set; } = null!;
	public DbSet<FileData> Files { get; internal set; } = null!;

	public async Task<IFilesManagementTransactionalWriteContext> BeginTransactionAsync()
		=> new FilesManagementTransactionalWriteContext(
			this,
			await Database.BeginTransactionAsync().ConfigureAwait(false),
			FilesManagementDbExceptionWrapper.Create()
		);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		Preconditions.RequiresNotNull(modelBuilder, nameof(modelBuilder));
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<StorageData>(
			entity =>
			{
				entity.HasMany<DirectoryData>(e => e.Directories)
					.WithOne()
					.HasForeignKey(e => e.StorageId)
					.OnDelete(DeleteBehavior.Cascade);
			}
		);

		modelBuilder.Entity<DirectoryData>(
			entity =>
			{
				entity.HasMany<FileData>(e => e.Files)
					.WithOne()
					.HasForeignKey(e => e.DirectoryId)
					.OnDelete(DeleteBehavior.Cascade);
			}
		);
	}
}