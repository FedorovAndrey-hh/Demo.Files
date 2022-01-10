using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public static class IdentityResultExtensions
{
	public static UserError? FirstUserError(this IdentityResult @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		foreach (var identityError in @this.Errors)
		{
			if (Enum.TryParse<UserError>(identityError.Code, out var result))
			{
				return result;
			}
		}

		return null;
	}

	public static void ThrowIfFailed(this IdentityResult @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (!@this.Succeeded)
		{
			var error = @this.FirstUserError();
			if (error.HasValue)
			{
				throw new UserException(error.Value);
			}
			else if (@this
			         .Errors
			         .Any(e => string.Equals(e.Code, nameof(IdentityErrorDescriber.ConcurrencyFailure))))
			{
				throw new UserException(UserError.Outdated);
			}
			else
			{
				Contracts.Assert(
					@this.Succeeded,
					"Identity policy must match domain rules. "
					+ string.Join(", ", @this.Errors.Select(e => $"({e.Code}: {e.Description})"))
				);
			}
		}
	}
}