using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Local.ContainerAggregate;

public sealed record ContainerId : IContainerId
{
	public static ContainerId Of(Guid value) => new(value);

	public static ContainerId Create() => new(Guid.NewGuid());

	private ContainerId(Guid value) => Value = value;

	public Guid Value { get; }

	public bool Equals(IContainerId? other) => other is ContainerId otherTyped && Equals(otherTyped);
}