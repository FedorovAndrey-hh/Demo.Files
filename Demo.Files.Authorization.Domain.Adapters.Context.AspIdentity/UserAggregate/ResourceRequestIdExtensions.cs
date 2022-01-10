using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public static class ResourceRequestIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static ResourceRequestId? Concrete(this IResourceRequestId? @this) => (ResourceRequestId?)@this;

	public static long RawLong(this IResourceRequestId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}