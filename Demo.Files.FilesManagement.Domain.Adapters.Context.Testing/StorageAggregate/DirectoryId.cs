using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public sealed record DirectoryId : IDirectoryId
{
	public static DirectoryId Of(long value) => new(value);

	private DirectoryId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IDirectoryId? other) => other is DirectoryId typedOther && Equals(typedOther);
}