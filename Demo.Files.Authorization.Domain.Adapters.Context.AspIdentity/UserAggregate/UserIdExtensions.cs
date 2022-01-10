using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public static class UserIdExtensions
{
	[return: NotNullIfNotNull("this")]
	public static UserId? Concrete(this IUserId? @this) => (UserId?)@this;

	public static long RawLong(this IUserId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Concrete().Value;
	}
}