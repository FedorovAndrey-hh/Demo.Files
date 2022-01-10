using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public sealed record StorageId : IStorageId
{
	public static StorageId Of(long value) => new(value);

	private StorageId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IStorageId? other) => other is StorageId otherTyped && Equals(otherTyped);
}