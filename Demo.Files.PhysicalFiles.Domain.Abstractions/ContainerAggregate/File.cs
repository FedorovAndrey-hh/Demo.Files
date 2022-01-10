using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Modifications;

namespace Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

public sealed class File : IIdentifiable<IFileId>
{
	internal File(IFileId id, DataSize<ulong> size)
	{
		Preconditions.RequiresNotNull(size, nameof(size));

		Id = id;
		Size = size;
	}

	public IFileId Id { get; }

	public DataSize<ulong> Size { get; }
}