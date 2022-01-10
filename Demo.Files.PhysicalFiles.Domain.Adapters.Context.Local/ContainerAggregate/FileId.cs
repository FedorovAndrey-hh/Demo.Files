using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Local.ContainerAggregate;

public sealed record FileId : IFileId
{
	public static FileId Of(Guid value) => new(value);

	public static FileId Create() => new(Guid.NewGuid());

	private FileId(Guid value) => Value = value;

	public Guid Value { get; }

	public bool Equals(IFileId? other) => other is FileId otherTyped && Equals(otherTyped);
}