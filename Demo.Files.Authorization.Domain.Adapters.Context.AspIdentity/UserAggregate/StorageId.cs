using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public sealed record StorageId : IStorageId
{
	public static StorageId Of(long value) => new(value);

	private StorageId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IStorageId? other) => other is StorageId otherTyped && Equals(otherTyped);
}