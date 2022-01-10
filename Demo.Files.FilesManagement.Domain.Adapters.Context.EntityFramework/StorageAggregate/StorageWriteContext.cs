using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

internal sealed class StorageWriteContext : Storage.IWriteContext
{
	internal StorageWriteContext(FilesManagementDbContext dbContext, IExceptionWrapper exceptionWrapper)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(exceptionWrapper, nameof(exceptionWrapper));

		_dbContext = dbContext;
		_exceptionWrapper = exceptionWrapper;
	}

	private readonly FilesManagementDbContext _dbContext;
	private readonly IExceptionWrapper _exceptionWrapper;

	async Task<StorageEvent.Created> Storage.IWriteContext.CreateAsync(Limitations limitations)
	{
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		try
		{
			var storageData = new StorageData();

			var version = StorageVersion.Initial;
			storageData.SetVersion(version);
			storageData.SetLimitations(limitations);

			var storageEntry = _dbContext.Storages.Add(storageData);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Created(storageEntry.Entity.GetId(), version, limitations);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.LimitationsChanged> Storage.IWriteContext.ChangeLimitationsAsync(
		Storage storage,
		Limitations limitations)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		try
		{
			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			storageData.SetLimitations(limitations);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.LimitationsChanged(
				storage.Id,
				storageData.GetVersion(),
				limitations
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.DirectoryAdded> Storage.IWriteContext.AddDirectoryAsync(
		Storage storage,
		DirectoryName directoryName)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryName, nameof(directoryName));

		try
		{
			var storageId = storage.Id.Concrete();

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var directoryData = new DirectoryData();
			directoryData.SetStorageId(storageId);
			directoryData.SetName(directoryName);

			var addedDirectoryEntry = _dbContext.Directories.Add(directoryData);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.DirectoryAdded(
				storage.Id,
				storageData.GetVersion(),
				addedDirectoryEntry.Entity.GetId(),
				directoryName
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.DirectoryRenamed> Storage.IWriteContext.RenameDirectoryAsync(
		Storage storage,
		IDirectoryId directoryId,
		DirectoryName newDirectoryName)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(newDirectoryName, nameof(newDirectoryName));

		try
		{
			var rawDirectoryId = ((DirectoryId)directoryId).Value;

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var directoryData =
				await _dbContext.Directories
					.FirstOrDefaultAsync(e => e.Id == rawDirectoryId)
					.ConfigureAwait(false)
				?? throw new StorageException(StorageError.DirectoryNotExists);

			directoryData.SetName(newDirectoryName);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.DirectoryRenamed(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				newDirectoryName
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.DirectoryRelocated> Storage.IWriteContext.RelocateDirectoryAsync(
		Storage storage,
		IDirectoryId directoryId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

		try
		{
			var rawDirectoryId = ((DirectoryId)directoryId).Value;

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var directoryData =
				await _dbContext.Directories
					.FirstOrDefaultAsync(e => e.Id == rawDirectoryId)
					.ConfigureAwait(false)
				?? throw new StorageException(StorageError.DirectoryNotExists);
			_dbContext.Directories.Remove(directoryData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			directoryData.ResetId();
			_dbContext.Directories.Add(directoryData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.DirectoryRelocated(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				directoryData.GetId()
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.DirectoryRemoved> Storage.IWriteContext.RemoveDirectoryAsync(
		Storage storage,
		IDirectoryId directoryId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

		try
		{
			var rawDirectoryId = ((DirectoryId)directoryId).Value;

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var directoryData =
				await _dbContext.Directories
					.FirstOrDefaultAsync(e => e.Id == rawDirectoryId)
					.ConfigureAwait(false)
				?? throw new StorageException(StorageError.DirectoryNotExists);
			_dbContext.Directories.Remove(directoryData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.DirectoryRemoved(
				storage.Id,
				storageData.GetVersion(),
				directoryId
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.FileAdded> Storage.IWriteContext.AddFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IPhysicalFileId filePhysicalId,
		FileName fileName,
		DataSize<ulong> fileSize)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(filePhysicalId, nameof(filePhysicalId));
		Preconditions.RequiresNotNull(fileName, nameof(fileName));
		Preconditions.RequiresNotNull(fileSize, nameof(fileSize));

		try
		{
			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var fileData = new FileData();
			fileData.SetPhysicalId(filePhysicalId.Concrete());
			fileData.SetDirectoryId((DirectoryId)directoryId);
			fileData.SetName(fileName);
			fileData.SetSize(fileSize);

			_dbContext.Files.Add(fileData);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.FileAdded(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				fileData.GetId(),
				filePhysicalId,
				fileName,
				fileSize
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.FileRenamed> Storage.IWriteContext.RenameFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId,
		FileName newFileName)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));
		Preconditions.RequiresNotNull(newFileName, nameof(newFileName));

		try
		{
			var rawFileId = fileId.RawLong();

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var fileData =
				await _dbContext.Files.FirstOrDefaultAsync(e => e.Id == rawFileId)
					.ConfigureAwait(false)
				?? throw new StorageException(StorageError.FileNotExists);

			fileData.SetName(newFileName);

			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.FileRenamed(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				fileId,
				newFileName
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.FileMoved> Storage.IWriteContext.MoveFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId,
		IDirectoryId destinationDirectoryId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));
		Preconditions.RequiresNotNull(destinationDirectoryId, nameof(destinationDirectoryId));

		try
		{
			var rawFileId = fileId.RawLong();

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var fileData =
				await _dbContext.Files
					.FirstOrDefaultAsync(e => e.Id == rawFileId)
					.ConfigureAwait(false)
				?? throw new StorageException(StorageError.FileNotExists);
			_dbContext.Remove(fileData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			fileData.ResetId();
			fileData.SetDirectoryId((DirectoryId)destinationDirectoryId);
			_dbContext.Files.Add(fileData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.FileMoved(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				fileId,
				destinationDirectoryId,
				fileData.GetId()
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.FileRelocated> Storage.IWriteContext.RelocateFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));

		try
		{
			var rawFileId = fileId.RawLong();

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var fileData =
				await _dbContext.Files.FirstOrDefaultAsync(e => e.Id == rawFileId).ConfigureAwait(false)
				?? throw new StorageException(StorageError.FileNotExists);
			_dbContext.Remove(fileData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			fileData.ResetId();
			_dbContext.Files.Add(fileData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.FileRelocated(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				fileId,
				fileData.GetId()
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Modified.FileRemoved> Storage.IWriteContext.RemoveFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));

		try
		{
			var rawFileId = fileId.RawLong();

			var storageData = await _GetStorageDataAndIncrementVersionAsync(_dbContext, storage)
				.ConfigureAwait(false);

			var fileData =
				await _dbContext.Files.FirstOrDefaultAsync(e => e.Id == rawFileId).ConfigureAwait(false)
				?? throw new StorageException(StorageError.FileNotExists);
			_dbContext.Files.Remove(fileData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Modified.FileRemoved(
				storage.Id,
				storageData.GetVersion(),
				directoryId,
				fileId
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<StorageEvent.Deleted> Storage.IWriteContext.DeleteAsync(Storage storage)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));

		var rawStorageId = storage.Id.RawLong();

		try
		{
			var storageData =
				await _dbContext.Storages.FirstOrDefaultAsync(e => e.Id == rawStorageId).ConfigureAwait(false)
				?? throw new StorageException(StorageError.NotExists);
			StorageDataUtility.SetOriginalVersion(_dbContext.Entry(storageData), storage.Version);

			_dbContext.Storages.Remove(storageData);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			return new StorageEvent.Deleted(storage.Id);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	private static async Task<StorageData> _GetStorageDataAndIncrementVersionAsync(
		FilesManagementDbContext dbContext,
		Storage storage)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(storage, nameof(storage));

		var rawStorageId = storage.Id.RawLong();

		var storageData =
			await dbContext.Storages.FirstOrDefaultAsync(e => e.Id == rawStorageId).ConfigureAwait(false)
			?? throw new StorageException(StorageError.NotExists);

		var newVersion = storage.Version.Increment();
		storageData.SetVersion(newVersion);
		StorageDataUtility.SetOriginalVersion(dbContext.Entry(storageData), storage.Version);

		return storageData;
	}
}