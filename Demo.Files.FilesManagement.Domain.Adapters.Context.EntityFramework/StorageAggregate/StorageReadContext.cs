using System.Collections.Immutable;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public sealed class StorageReadContext : Storage.IReadContext
{
	public StorageReadContext(FilesManagementDbContext dbContext)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));

		_dbContext = dbContext;
	}

	private readonly FilesManagementDbContext _dbContext;

	async Task<Storage?> Storage.IReadContext.FindAsync(IStorageId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		var rawId = id.RawLong();
		var storageData =
			await _dbContext.Storages
				.AsNoTracking()
				.Include(e => e.Directories)
				.ThenInclude(e => e.Files)
				.FirstOrDefaultAsync(e => e.Id == rawId)
				.ConfigureAwait(false)
			?? throw new StorageException(StorageError.NotExists);

		return Storage.IReadContext.FromSnapshot(
			storageData.GetId(),
			storageData.GetVersion(),
			storageData.GetLimitations(),
			storageData.Directories
				.Select(
					directoryData => Storage.IReadContext.FromSnapshot(
						directoryData.GetId(),
						directoryData.GetName(),
						directoryData.Files
							.Select(
								fileData => Storage.IReadContext.FromSnapshot(
									fileData.GetId(),
									fileData.GetPhysicalId(),
									fileData.GetName(),
									fileData.GetSize()
								)
							)
							.ToImmutableList()
					)
				)
				.ToImmutableList()
		);
	}
}