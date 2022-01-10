using Common.Core.Data;
using Common.Core.Diagnostics;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public abstract class StorageEvent
{
	private StorageEvent(IStorageId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		Id = id;
	}

	public IStorageId Id { get; }

	public sealed class Created : StorageEvent
	{
		public Created(IStorageId id, StorageVersion version, Limitations limitations)
			: base(id)
		{
			Preconditions.RequiresNotNull(version, nameof(version));
			Preconditions.RequiresNotNull(limitations, nameof(limitations));

			Version = version;
			Limitations = limitations;
		}

		public StorageVersion Version { get; }

		public Limitations Limitations { get; }
	}

	public sealed class Deleted : StorageEvent
	{
		public Deleted(IStorageId id)
			: base(id)
		{
		}
	}

	public abstract class Modified : StorageEvent
	{
		private Modified(IStorageId id, StorageVersion newVersion)
			: base(id)
		{
			Preconditions.RequiresNotNull(newVersion, nameof(newVersion));

			NewVersion = newVersion;
		}

		public StorageVersion NewVersion { get; }

		public sealed class LimitationsChanged : Modified
		{
			public LimitationsChanged(
				IStorageId id,
				StorageVersion newVersion,
				Limitations limitations)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(limitations, nameof(limitations));

				Limitations = limitations;
			}

			public Limitations Limitations { get; }
		}

		public sealed class DirectoryAdded : Modified
		{
			public DirectoryAdded(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				DirectoryName directoryName)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(directoryName, nameof(directoryName));

				DirectoryId = directoryId;
				DirectoryName = directoryName;
			}

			public IDirectoryId DirectoryId { get; }

			public DirectoryName DirectoryName { get; }
		}

		public sealed class DirectoryRenamed : Modified
		{
			public DirectoryRenamed(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				DirectoryName newDirectoryName)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(newDirectoryName, nameof(newDirectoryName));

				DirectoryId = directoryId;
				NewDirectoryName = newDirectoryName;
			}

			public IDirectoryId DirectoryId { get; }

			public DirectoryName NewDirectoryName { get; }
		}

		public sealed class DirectoryRelocated : Modified
		{
			public DirectoryRelocated(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				IDirectoryId newDirectoryId)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(newDirectoryId, nameof(newDirectoryId));

				DirectoryId = directoryId;
				NewDirectoryId = newDirectoryId;
			}

			public IDirectoryId DirectoryId { get; }

			public IDirectoryId NewDirectoryId { get; }
		}

		public sealed class DirectoryRemoved : Modified
		{
			public DirectoryRemoved(IStorageId id, StorageVersion newVersion, IDirectoryId directoryId)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

				DirectoryId = directoryId;
			}

			public IDirectoryId DirectoryId { get; }
		}

		public sealed class FileAdded : Modified
		{
			public FileAdded(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				IFileId fileId,
				IPhysicalFileId filePhysicalId,
				FileName fileName,
				DataSize<ulong> fileSize)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(fileId, nameof(fileId));
				Preconditions.RequiresNotNull(filePhysicalId, nameof(filePhysicalId));
				Preconditions.RequiresNotNull(fileName, nameof(fileName));
				Preconditions.RequiresNotNull(fileSize, nameof(fileSize));

				DirectoryId = directoryId;
				FileId = fileId;
				FilePhysicalId = filePhysicalId;
				FileName = fileName;
				FileSize = fileSize;
			}

			public IDirectoryId DirectoryId { get; }

			public IFileId FileId { get; }
			public IPhysicalFileId FilePhysicalId { get; }
			public FileName FileName { get; }
			public DataSize<ulong> FileSize { get; }
		}

		public sealed class FileRenamed : Modified
		{
			public FileRenamed(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				IFileId fileId,
				FileName newFileName)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(fileId, nameof(fileId));
				Preconditions.RequiresNotNull(newFileName, nameof(newFileName));

				DirectoryId = directoryId;
				NewFileName = newFileName;
				FileId = fileId;
			}

			public IDirectoryId DirectoryId { get; }

			public IFileId FileId { get; }
			public FileName NewFileName { get; }
		}

		public sealed class FileMoved : Modified
		{
			public FileMoved(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				IFileId fileId,
				IDirectoryId newDirectoryId,
				IFileId newFileId)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(fileId, nameof(fileId));
				Preconditions.RequiresNotNull(newDirectoryId, nameof(newDirectoryId));
				Preconditions.RequiresNotNull(newFileId, nameof(newFileId));

				DirectoryId = directoryId;
				FileId = fileId;
				NewDirectoryId = newDirectoryId;
				NewFileId = newFileId;
			}

			public IDirectoryId DirectoryId { get; }
			public IFileId FileId { get; }

			public IDirectoryId NewDirectoryId { get; }
			public IFileId NewFileId { get; }
		}

		public sealed class FileRelocated : Modified
		{
			public FileRelocated(
				IStorageId id,
				StorageVersion newVersion,
				IDirectoryId directoryId,
				IFileId fileId,
				IFileId newFileId)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(fileId, nameof(fileId));
				Preconditions.RequiresNotNull(newFileId, nameof(newFileId));

				DirectoryId = directoryId;
				FileId = fileId;
				NewFileId = newFileId;
			}

			public IDirectoryId DirectoryId { get; }
			public IFileId FileId { get; }

			public IFileId NewFileId { get; }
		}

		public sealed class FileRemoved : Modified
		{
			public FileRemoved(IStorageId id, StorageVersion newVersion, IDirectoryId directoryId, IFileId fileId)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
				Preconditions.RequiresNotNull(fileId, nameof(fileId));

				DirectoryId = directoryId;
				FileId = fileId;
			}

			public IDirectoryId DirectoryId { get; }
			public IFileId FileId { get; }
		}
	}
}