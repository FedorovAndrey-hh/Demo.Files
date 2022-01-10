namespace Demo.Files.Common.Contracts.Communication;

public sealed record StorageId
{
	public static StorageId Of(long value) => new(value);

	private StorageId(long value) => Value = value;

	public long Value { get; }
}