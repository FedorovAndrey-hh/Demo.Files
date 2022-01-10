using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Testing.ContainerAggregate;

public static class ContainerIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static ContainerId? Concrete(this IContainerId? @this) => (ContainerId?)@this;

	public static ulong RawUlong(this IContainerId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}