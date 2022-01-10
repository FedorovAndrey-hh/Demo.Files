using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.MathConcepts.GroupKind;
using Common.Core.Modifications;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed record Storage
	: IIdentifiable<IStorageId>,
	  IVersionable<StorageVersion>,
	  IContinuous<Storage, StorageEvent.Modified>
{
	public static Task<StorageEvent.Created> CreateAsync(IWriteContext context, Limitations limitations)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		return context.CreateAsync(limitations);
	}

	[return: NotNullIfNotNull("event")]
	public static Storage? After(StorageEvent.Created? @event)
	{
		if (@event is null)
		{
			return null;
		}

		if (!Eq.ValueSafe(@event.Version, StorageVersion.Initial))
		{
			throw new StorageException(StorageError.InvalidHistory);
		}

		return new Storage(
			@event.Id,
			@event.Version,
			@event.Limitations,
			ImmutableList.Create<Directory>()
		);
	}

	public Storage After(StorageEvent.Modified? @event)
	{
		if (@event is null)
		{
			return this;
		}

		if (!Eq.ValueSafe(@event.Id, Id) || !@event.NewVersion.IsIncrementOf(Version))
		{
			throw new StorageException(StorageError.InvalidHistory);
		}

		return @event switch
		{
			StorageEvent.Modified.LimitationsChanged e => _AfterLimitationsChanged(e),
			StorageEvent.Modified.DirectoryAdded e => _AfterDirectoryAdded(e),
			StorageEvent.Modified.DirectoryRenamed e => _AfterDirectoryRenamed(e),
			StorageEvent.Modified.DirectoryRelocated e => _AfterDirectoryRelocated(e),
			StorageEvent.Modified.DirectoryRemoved e => _AfterDirectoryRemoved(e),
			StorageEvent.Modified.FileAdded e => _AfterFileAdded(e),
			StorageEvent.Modified.FileRenamed e => _AfterFileRenamed(e),
			StorageEvent.Modified.FileMoved e => _AfterFileMoved(e),
			StorageEvent.Modified.FileRelocated e => _AfterFileRelocated(e),
			StorageEvent.Modified.FileRemoved e => _AfterFileRemoved(e),
			_ => throw new StorageException(StorageError.InvalidHistory)
		};
	}

	private Storage(
		IStorageId id,
		StorageVersion version,
		Limitations limitations,
		IImmutableList<Directory> directories)
	{
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(version, nameof(version));
		Preconditions.RequiresNotNull(limitations, nameof(limitations));
		Preconditions.RequiresNotNull(directories, nameof(directories));

		Id = id;
		Version = version;
		Limitations = limitations;
		Directories = directories;
	}

	public IStorageId Id { get; }
	public StorageVersion Version { get; private init; }
	public Limitations Limitations { get; private init; }

	public bool IsLimitationsExceeded
	{
		get
		{
			if (Limitations.TotalFileCount < Metrics.FilesCount)
			{
				return true;
			}

			if (Limitations.TotalSpace.LessThan(Metrics.Size, DataSizeComparer))
			{
				return true;
			}

			if (Directories
			    .SelectMany(e => e.Files)
			    .Any(e => Limitations.SingleFileSize.LessThan(e.Size, DataSizeComparer)))
			{
				return true;
			}

			return false;
		}
	}

	public Metrics Metrics => Directories.Select(e => e.Metrics).Aggregate(Metrics.Monoid);

	public IImmutableList<Directory> Directories { get; private init; }

	public Directory Directory(IDirectoryId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		return FindDirectory(id) ?? throw new StorageException(StorageError.DirectoryNotExists);
	}

	public Directory? FindDirectory(IDirectoryId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		return Directories.FirstOrDefault(e => Eq.ValueSafe(e.Id, id));
	}

	public Directory Directory(DirectoryName name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return FindDirectory(name) ?? throw new StorageException(StorageError.DirectoryNotExists);
	}

	public Directory? FindDirectory(DirectoryName name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return Directories.FirstOrDefault(e => Eq.ValueSafe(e.Name, name));
	}

	public async Task<StorageEvent.Modified.LimitationsChanged?> ChangeLimitationsAsync(
		IWriteContext context,
		Limitations limitations)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		if (Eq.ValueSafe(Limitations, limitations))
		{
			return null;
		}

		return await context
			.ChangeLimitationsAsync(
				this,
				limitations
			)
			.ConfigureAwait(false);
	}

	private Storage _AfterLimitationsChanged(StorageEvent.Modified.LimitationsChanged @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with { Limitations = @event.Limitations };
	}

	public async Task<StorageEvent.Modified.DirectoryAdded> AddDirectoryAsync(IWriteContext context, DirectoryName name)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(name, nameof(name));

		if (Directories.Any(e => Eq.ValueSafe(e.Name, name)))
		{
			throw new StorageException(StorageError.DirectoryNameConflict);
		}

		return await context
			.AddDirectoryAsync(this, name)
			.ConfigureAwait(false);
	}

	private Storage _AfterDirectoryAdded(StorageEvent.Modified.DirectoryAdded @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories.Add(
				new Directory(@event.DirectoryId, @event.DirectoryName, ImmutableList.Create<File>())
			)
		};
	}

	public async Task<StorageEvent.Modified.DirectoryRenamed?> RenameDirectoryAsync(
		IWriteContext context,
		IDirectoryId id,
		DirectoryName name)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(name, nameof(name));

		var directory = Directory(id);

		if (Eq.ValueSafe(directory.Name, name))
		{
			return null;
		}

		if (Directories.Any(e => !e.IdentityEquals(id) && Eq.ValueSafe(e.Name, name)))
		{
			throw new StorageException(StorageError.DirectoryNameConflict);
		}

		return await context
			.RenameDirectoryAsync(
				this,
				id,
				name
			)
			.ConfigureAwait(false);
	}

	private Storage _AfterDirectoryRenamed(StorageEvent.Modified.DirectoryRenamed @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories.ReplaceById<Directory, IDirectoryId>(
				directory with { Name = @event.NewDirectoryName }
			)
		};
	}

	public async Task<StorageEvent.Modified.DirectoryRelocated> RelocateDirectoryAsync(
		IWriteContext context,
		IDirectoryId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));

		if (Directories.None(e => Eq.ValueSafe(e.Id, id)))
		{
			throw new StorageException(StorageError.DirectoryNotExists);
		}

		return await context
			.RelocateDirectoryAsync(this, id)
			.ConfigureAwait(false);
	}

	private Storage _AfterDirectoryRelocated(StorageEvent.Modified.DirectoryRelocated @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);

		return this with
		{
			Directories = Directories
				.RemoveById<Directory, IDirectoryId>(directory)
				.Add(directory with { Id = @event.NewDirectoryId })
		};
	}

	public async Task<StorageEvent.Modified.DirectoryRemoved> RemoveDirectoryAsync(
		IWriteContext context,
		IDirectoryId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));

		if (Directories.None(e => Eq.ValueSafe(e.Id, id)))
		{
			throw new StorageException(StorageError.DirectoryNotExists);
		}

		return await context.RemoveDirectoryAsync(this, id).ConfigureAwait(false);
	}

	private Storage _AfterDirectoryRemoved(StorageEvent.Modified.DirectoryRemoved @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories.RemoveAll(e => Eq.ValueSafe(e.Id, @event.DirectoryId))
		};
	}

	public async Task<StorageEvent.Modified.FileAdded> AddFileAsync(
		IWriteContext context,
		IDirectoryId directoryId,
		IPhysicalFileId filePhysicalId,
		FileName name,
		DataSize<ulong> size)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(filePhysicalId, nameof(filePhysicalId));
		Preconditions.RequiresNotNull(name, nameof(name));
		Preconditions.RequiresNotNull(size, nameof(size));

		if (!_CanAddFileOfSize(size))
		{
			throw new StorageException(StorageError.ExceededLimitations);
		}

		var directory = Directory(directoryId);

		if (directory.Files.Any(e => Eq.ValueSafe(e.Name, name)))
		{
			throw new StorageException(StorageError.FileNameConflict);
		}

		return await context
			.AddFileAsync(
				this,
				directoryId,
				filePhysicalId,
				name,
				size
			)
			.ConfigureAwait(false);
	}

	private Storage _AfterFileAdded(StorageEvent.Modified.FileAdded @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories.ReplaceById<Directory, IDirectoryId>(
				directory with
				{
					Files = directory.Files.Add(
						new File(
							@event.FileId,
							@event.FilePhysicalId,
							@event.FileName,
							@event.FileSize
						)
					)
				}
			)
		};
	}

	public async Task<StorageEvent.Modified.FileRenamed?> RenameFileAsync(
		IWriteContext context,
		IDirectoryId directoryId,
		IFileId fileId,
		FileName name)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));
		Preconditions.RequiresNotNull(name, nameof(name));

		var directory = Directory(directoryId);
		var file = directory.File(fileId);

		if (Eq.ValueSafe(file.Name, name))
		{
			return null;
		}

		if (directory.Files.Any(e => !e.IdentityEquals(fileId) && Eq.ValueSafe(e.Name, name)))
		{
			throw new StorageException(StorageError.FileNameConflict);
		}

		return await context
			.RenameFileAsync(
				this,
				directoryId,
				fileId,
				name
			)
			.ConfigureAwait(false);
	}

	private Storage _AfterFileRenamed(StorageEvent.Modified.FileRenamed @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);
		var file = directory.File(@event.FileId);

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories.ReplaceById<Directory, IDirectoryId>(
				directory with
				{
					Files = directory.Files.ReplaceById<File, IFileId>(file with { Name = @event.NewFileName })
				}
			)
		};
	}

	public async Task<StorageEvent.Modified.FileMoved> MoveFileAsync(
		IWriteContext context,
		IDirectoryId directoryId,
		IFileId fileId,
		IDirectoryId destinationDirectoryId)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));
		Preconditions.RequiresNotNull(destinationDirectoryId, nameof(destinationDirectoryId));

		if (Eq.ValueSafe(directoryId, destinationDirectoryId))
		{
			throw new StorageException(StorageError.FileIllegalMove);
		}

		var directory = Directory(directoryId);
		var file = directory.File(fileId);

		var destinationDirectory = Directory(destinationDirectoryId);

		if (destinationDirectory.Files.Any(e => Eq.ValueSafe(e.Name, file.Name)))
		{
			throw new StorageException(StorageError.FileNameConflict);
		}

		return await context
			.MoveFileAsync(
				this,
				directoryId,
				fileId,
				destinationDirectoryId
			)
			.ConfigureAwait(false);
	}

	private Storage _AfterFileMoved(StorageEvent.Modified.FileMoved @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);
		var file = directory.File(@event.FileId);

		var destinationDirectory = Directory(@event.NewDirectoryId);
		var movedFile = file with { Id = @event.NewFileId };

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories
				.ReplaceById<Directory, IDirectoryId>(
					directory with
					{
						Files = directory.Files.RemoveAll(e => Eq.ValueSafe(e.Id, @event.FileId))
					}
				)
				.ReplaceById<Directory, IDirectoryId>(
					destinationDirectory with
					{
						Files = destinationDirectory.Files.Add(movedFile)
					}
				)
		};
	}

	public async Task<StorageEvent.Modified.FileRelocated> RelocateFileAsync(
		IWriteContext context,
		IDirectoryId directoryId,
		IFileId fileId)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));

		var directory = Directory(directoryId);

		if (directory.Files.None(e => Eq.ValueSafe(e.Id, fileId)))
		{
			throw new StorageException(StorageError.FileNotExists);
		}

		return await context
			.RelocateFileAsync(
				this,
				directoryId,
				fileId
			)
			.ConfigureAwait(false);
	}

	private Storage _AfterFileRelocated(StorageEvent.Modified.FileRelocated @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);
		var file = directory.File(@event.FileId);

		return this with
		{
			Directories = Directories.ReplaceById<Directory, IDirectoryId>(
				directory with
				{
					Files = directory.Files
						.RemoveById<File, IFileId>(file)
						.Add(file with { Id = @event.NewFileId })
				}
			)
		};
	}

	public async Task<StorageEvent.Modified.FileRemoved> RemoveFileAsync(
		IWriteContext context,
		IDirectoryId directoryId,
		IFileId fileId)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(fileId, nameof(fileId));

		var directory = Directory(directoryId);

		if (directory.Files.None(e => Eq.ValueSafe(e.Id, fileId)))
		{
			throw new StorageException(StorageError.FileNotExists);
		}

		return await context
			.RemoveFileAsync(this, directoryId, fileId)
			.ConfigureAwait(false);
	}

	private Storage _AfterFileRemoved(StorageEvent.Modified.FileRemoved @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var directory = Directory(@event.DirectoryId);

		return this with
		{
			Version = @event.NewVersion,
			Directories = Directories.ReplaceById<Directory, IDirectoryId>(
				directory with { Files = directory.Files.RemoveAll(e => Eq.ValueSafe(e.Id, @event.FileId)) }
			)
		};
	}

	public Task<StorageEvent.Deleted> DeleteAsync(IWriteContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		return context.DeleteAsync(this);
	}

	private bool _CanAddFileOfSize(DataSize<ulong> size)
	{
		Preconditions.RequiresNotNull(size, nameof(size));

		if (Limitations.SingleFileSize.LessThan(size, DataSizeComparer))
		{
			return false;
		}

		var metricsWithFile = Metrics.Monoid.Combine(Metrics, Metrics.OfSingleFile(size));

		if (Limitations.TotalFileCount < metricsWithFile.FilesCount)
		{
			return false;
		}

		if (Limitations.TotalSpace.LessThan(metricsWithFile.Size, DataSizeComparer))
		{
			return false;
		}

		return true;
	}

	public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);

	public bool Equals(Storage? other) => ReferenceEquals(this, other);

	public static Task<Storage?> FindAsync(IReadContext context, IStorageId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));

		return context.FindAsync(id);
	}

	public static async Task<Storage> GetAsync(IReadContext context, IStorageId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));

		return await context.FindAsync(id).ConfigureAwait(false)
		       ?? throw new StorageException(StorageError.NotExists);
	}

	public interface IReadContext
	{
		protected internal Task<Storage?> FindAsync(IStorageId id);

		protected static Storage FromHistory(IImmutableList<StorageEvent> events)
		{
			Preconditions.RequiresNotNull(events, nameof(events));

			using var history = events.GetEnumerator();

			var created = history.MoveNext() ? history.Current as StorageEvent.Created : null;
			if (created is null)
			{
				throw new StorageException(StorageError.InvalidHistory);
			}

			var result = After(created);
			while (history.MoveNext())
			{
				var modified = history.Current as StorageEvent.Modified
				               ?? throw new StorageException(StorageError.InvalidHistory);

				result = result.After(modified);
			}

			return result;
		}

		protected static Storage FromSnapshot(
			IStorageId id,
			StorageVersion version,
			Limitations limitations,
			IImmutableList<Directory> directories)
		{
			Preconditions.RequiresNotNull(id, nameof(id));
			Preconditions.RequiresNotNull(version, nameof(version));
			Preconditions.RequiresNotNull(limitations, nameof(limitations));
			Preconditions.RequiresNotNull(directories, nameof(directories));

			return new Storage(
				id,
				version,
				limitations,
				directories
			);
		}

		protected static Directory FromSnapshot(
			IDirectoryId id,
			DirectoryName name,
			IImmutableList<File> files)
		{
			Preconditions.RequiresNotNull(name, nameof(name));
			Preconditions.RequiresNotNull(files, nameof(files));

			return new Directory(id, name, files);
		}

		protected static File FromSnapshot(
			IFileId id,
			IPhysicalFileId physicalId,
			FileName name,
			DataSize<ulong> size)
		{
			Preconditions.RequiresNotNull(id, nameof(id));
			Preconditions.RequiresNotNull(physicalId, nameof(physicalId));
			Preconditions.RequiresNotNull(name, nameof(name));
			Preconditions.RequiresNotNull(size, nameof(size));

			return new File(
				id,
				physicalId,
				name,
				size
			);
		}
	}

	public interface IWriteContext
	{
		protected internal Task<StorageEvent.Created> CreateAsync(Limitations limitations);

		protected internal Task<StorageEvent.Deleted> DeleteAsync(Storage storage);

		protected internal Task<StorageEvent.Modified.LimitationsChanged> ChangeLimitationsAsync(
			Storage storage,
			Limitations limitations);

		protected internal Task<StorageEvent.Modified.DirectoryAdded> AddDirectoryAsync(
			Storage storage,
			DirectoryName directoryName);

		protected internal Task<StorageEvent.Modified.DirectoryRenamed> RenameDirectoryAsync(
			Storage storage,
			IDirectoryId directoryId,
			DirectoryName newDirectoryName);

		protected internal Task<StorageEvent.Modified.DirectoryRelocated> RelocateDirectoryAsync(
			Storage storage,
			IDirectoryId directoryId);

		protected internal Task<StorageEvent.Modified.DirectoryRemoved> RemoveDirectoryAsync(
			Storage storage,
			IDirectoryId directoryId);

		protected internal Task<StorageEvent.Modified.FileAdded> AddFileAsync(
			Storage storage,
			IDirectoryId directoryId,
			IPhysicalFileId filePhysicalId,
			FileName fileName,
			DataSize<ulong> fileSize);

		protected internal Task<StorageEvent.Modified.FileRenamed> RenameFileAsync(
			Storage storage,
			IDirectoryId directoryId,
			IFileId fileId,
			FileName newFileName);

		protected internal Task<StorageEvent.Modified.FileMoved> MoveFileAsync(
			Storage storage,
			IDirectoryId directoryId,
			IFileId fileId,
			IDirectoryId destinationDirectoryId);

		protected internal Task<StorageEvent.Modified.FileRelocated> RelocateFileAsync(
			Storage storage,
			IDirectoryId directoryId,
			IFileId fileId);

		protected internal Task<StorageEvent.Modified.FileRemoved> RemoveFileAsync(
			Storage storage,
			IDirectoryId directoryId,
			IFileId fileId);
	}

	public static IEqualityComparer<DataSize<ulong>> DataSizeEqualityComparer { get; }
		= DataSize.EqualityComparers.UlongBytePrecision;

	public static IComparer<DataSize<ulong>> DataSizeComparer { get; }
		= DataSize.Comparers.UlongBytePrecision;

	public static IMonoid<DataSize<ulong>> DataSizeMonoid { get; }
		= DataSize.Monoids.UlongAdditionBytePrecision;
}