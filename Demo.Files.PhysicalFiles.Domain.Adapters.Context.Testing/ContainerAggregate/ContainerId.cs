using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Testing.ContainerAggregate;

public sealed record ContainerId : IContainerId
{
	public static ContainerId Of(ulong value) => new(value);

	private ContainerId(ulong value) => Value = value;

	public ulong Value { get; }

	public bool Equals(IContainerId? other) => other is ContainerId otherTyped && Equals(otherTyped);
}