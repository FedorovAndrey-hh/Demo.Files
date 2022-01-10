using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Modifications;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed record Directory : IIdentifiable<IDirectoryId>
{
	internal Directory(IDirectoryId id, DirectoryName name, IImmutableList<File> files)
	{
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(name, nameof(name));
		Preconditions.RequiresNotNull(files, nameof(files));

		Id = id;
		Name = name;
		Files = files;
	}

	public IDirectoryId Id { get; internal init; }
	public DirectoryName Name { get; internal init; }

	public Metrics Metrics => Metrics.OfFiles(Files.Select(e => e.Size));

	public bool IsEmpty() => Files.Count == 0;

	internal IImmutableList<File> Files { get; init; }

	public File File(IFileId id) => FindFile(id) ?? throw new StorageException(StorageError.FileNotExists);

	public File? FindFile(IFileId id) => Files.FirstOrDefault(e => Eq.ValueSafe(e.Id, id));

	public File File(FileName name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return FindFile(name) ?? throw new StorageException(StorageError.FileNotExists);
	}

	public File? FindFile(FileName name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return Files.FirstOrDefault(e => Eq.ValueSafe(e.Name, name));
	}

	public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);

	public bool Equals(Directory? other) => ReferenceEquals(this, other);
}