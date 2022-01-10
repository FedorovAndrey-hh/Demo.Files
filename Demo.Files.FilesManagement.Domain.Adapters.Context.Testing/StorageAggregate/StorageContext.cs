using System.Collections.Immutable;
using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Modifications;
using Common.Core.Progressions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public sealed class StorageContext
	: Storage.IWriteContext,
	  Storage.IReadContext
{
	private readonly object _lock = new();
	private readonly Dictionary<StorageId, LinkedList<StorageEvent>> _data = new();

	private readonly IEnumerator<StorageId> _storageIdGenerator =
		new DelegateProgression<StorageId>(
			StorageId.Of(0),
			id => StorageId.Of(id.Value + 1)
		).GetEnumerator();

	private readonly IEnumerator<DirectoryId> _directoryIdGenerator =
		new DelegateProgression<DirectoryId>(
			DirectoryId.Of(0),
			id => DirectoryId.Of(id.Value + 1)
		).GetEnumerator();

	private readonly IEnumerator<FileId> _fileIdGenerator =
		new DelegateProgression<FileId>(
			FileId.Of(0),
			id => FileId.Of(id.Value + 1)
		).GetEnumerator();

	Task<StorageEvent.Created> Storage.IWriteContext.CreateAsync(Limitations limitations)
	{
		lock (_lock)
		{
			var id = _storageIdGenerator.Next();

			var @event = new StorageEvent.Created(id, StorageVersion.Initial, limitations);

			var history = new LinkedList<StorageEvent>();

			history.AddLast(@event);

			_data[id] = history;

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.LimitationsChanged> Storage.IWriteContext.ChangeLimitationsAsync(
		Storage storage,
		Limitations limitations)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.LimitationsChanged(
				storage.Id,
				storage.Version.Increment(),
				limitations
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.DirectoryAdded> Storage.IWriteContext.AddDirectoryAsync(
		Storage storage,
		DirectoryName directoryName)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryName, nameof(directoryName));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.DirectoryAdded(
				storage.Id,
				storage.Version.Increment(),
				_directoryIdGenerator.Next(),
				directoryName
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.DirectoryRenamed> Storage.IWriteContext.RenameDirectoryAsync(
		Storage storage,
		IDirectoryId directoryId,
		DirectoryName newDirectoryName)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(newDirectoryName, nameof(newDirectoryName));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.DirectoryRenamed(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				newDirectoryName
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.DirectoryRelocated> Storage.IWriteContext.RelocateDirectoryAsync(
		Storage storage,
		IDirectoryId directoryId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.DirectoryRelocated(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				_directoryIdGenerator.Next()
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.DirectoryRemoved> Storage.IWriteContext.RemoveDirectoryAsync(
		Storage storage,
		IDirectoryId directoryId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.DirectoryRemoved(
				storage.Id,
				storage.Version.Increment(),
				directoryId
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.FileAdded> Storage.IWriteContext.AddFileAsync(
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

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.FileAdded(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				_fileIdGenerator.Next(),
				filePhysicalId,
				fileName,
				fileSize
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.FileRenamed> Storage.IWriteContext.RenameFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId,
		FileName newFileName)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));
		Preconditions.RequiresNotNull(newFileName, nameof(newFileName));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.FileRenamed(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				fileId,
				newFileName
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.FileMoved> Storage.IWriteContext.MoveFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId,
		IDirectoryId destinationDirectoryId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));
		Preconditions.RequiresNotNull(destinationDirectoryId, nameof(destinationDirectoryId));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.FileMoved(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				fileId,
				destinationDirectoryId,
				_fileIdGenerator.Next()
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.FileRelocated> Storage.IWriteContext.RelocateFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.FileRelocated(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				fileId,
				_fileIdGenerator.Next()
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<StorageEvent.Modified.FileRemoved> Storage.IWriteContext.RemoveFileAsync(
		Storage storage,
		IDirectoryId directoryId,
		IFileId fileId)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));

		lock (_lock)
		{
			var @event = new StorageEvent.Modified.FileRemoved(
				storage.Id,
				storage.Version.Increment(),
				directoryId,
				fileId
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	private void _Update(StorageEvent.Modified @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		if (!_data.TryGetValue(@event.Id.Concrete(), out var history))
		{
			throw new StorageException(StorageError.NotExists);
		}

		var lastEvent = history.Last?.Value;
		var lastVersion =
			lastEvent is StorageEvent.Created created ? created.Version :
			lastEvent is StorageEvent.Modified modified ? modified.NewVersion :
			null;
		if (lastVersion is null || !@event.NewVersion.IsIncrementOf(lastVersion))
		{
			throw new StorageException(StorageError.Outdated);
		}

		history.AddLast(@event);
	}

	Task<Storage?> Storage.IReadContext.FindAsync(IStorageId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		lock (_lock)
		{
			return (_data.TryGetValue(id.Concrete(), out var history)
					? Storage.IReadContext.FromHistory(history.ToImmutableList())
					: null
				).AsTaskResult();
		}
	}

	Task<StorageEvent.Deleted> Storage.IWriteContext.DeleteAsync(Storage storage)
	{
		Preconditions.RequiresNotNull(storage, nameof(storage));

		lock (_lock)
		{
			var typedId = storage.Id.Concrete();

			if (!_data.TryGetValue(typedId, out var history))
			{
				throw new StorageException(StorageError.NotExists);
			}

			var lastEvent = history.Last?.Value;
			var lastVersion =
				lastEvent is StorageEvent.Created created ? created.Version :
				lastEvent is StorageEvent.Modified modified ? modified.NewVersion :
				null;
			if (lastVersion is null || !Eq.ValueSafe(storage.Version, lastVersion))
			{
				throw new StorageException(StorageError.Outdated);
			}

			_data.Remove(typedId);

			return new StorageEvent.Deleted(storage.Id).AsTaskResult();
		}
	}
}