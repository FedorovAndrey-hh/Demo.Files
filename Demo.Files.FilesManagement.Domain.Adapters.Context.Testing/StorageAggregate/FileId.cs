using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public sealed record FileId : IFileId
{
	public static FileId Of(long value) => new(value);

	private FileId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IFileId? other) => other is FileId typedOther && Equals(typedOther);
}