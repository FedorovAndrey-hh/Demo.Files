namespace Demo.Files.Common.Contracts.Communication;

public sealed record DirectoryId
{
	public static DirectoryId Of(long value) => new(value);

	private DirectoryId(long value) => Value = value;

	public long Value { get; }
}