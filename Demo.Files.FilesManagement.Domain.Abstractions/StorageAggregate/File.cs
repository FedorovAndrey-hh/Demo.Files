using System.Runtime.CompilerServices;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Modifications;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed record File : IIdentifiable<IFileId>
{
	internal File(IFileId id, IPhysicalFileId physicalId, FileName name, DataSize<ulong> size)
	{
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(physicalId, nameof(physicalId));
		Preconditions.RequiresNotNull(name, nameof(name));
		Preconditions.RequiresNotNull(size, nameof(size));

		Id = id;
		PhysicalId = physicalId;
		Name = name;
		Size = size;
	}

	public IFileId Id { get; internal init; }
	public IPhysicalFileId PhysicalId { get; }
	public FileName Name { get; internal init; }

	public DataSize<ulong> Size { get; }

	public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);

	public bool Equals(File? other) => ReferenceEquals(this, other);
}