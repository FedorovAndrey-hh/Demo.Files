using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Testing.ContainerAggregate;

public sealed record FileId : IFileId
{
	public static FileId Of(ulong value) => new(value);

	private FileId(ulong value) => Value = value;

	public ulong Value { get; }

	public bool Equals(IFileId? other) => other is FileId otherTyped && Equals(otherTyped);
}