using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public sealed class UserValidator : IUserValidator<UserData>
{
	private static UserValidator? _cache;

	public static UserValidator Create() => _cache ?? (_cache = new UserValidator());

	private UserValidator()
	{
	}

	public async Task<IdentityResult> ValidateAsync(UserManager<UserData> manager, UserData user)
	{
		Preconditions.RequiresNotNull(manager, nameof(manager));
		Preconditions.RequiresNotNull(user, nameof(user));

		var email = await manager.GetEmailAsync(user).ConfigureAwait(false);
		if (!UserEmail.Parser.Validate(email, out var emailError))
		{
			return IdentityResult.Failed(IdentityErrors.Of(emailError));
		}

		var userId = await manager.GetUserIdAsync(user).ConfigureAwait(false);

		var emailOwner = await manager.FindByEmailAsync(email).ConfigureAwait(false);
		if (emailOwner is not null
		    && !string.Equals(await manager.GetUserIdAsync(emailOwner).ConfigureAwait(false), userId))
		{
			return IdentityResult.Failed(IdentityErrors.Of(UserError.EmailConflict));
		}

		var rawUsername = await manager.GetUserNameAsync(user).ConfigureAwait(false);
		if (!Username.Parser.Validate(rawUsername, out var usernameError))
		{
			return IdentityResult.Failed(IdentityErrors.Of(usernameError));
		}

		var usernameOwner = await manager.FindByNameAsync(rawUsername).ConfigureAwait(false);
		if (usernameOwner is not null
		    && !string.Equals(await manager.GetUserIdAsync(usernameOwner).ConfigureAwait(false), userId))
		{
			return IdentityResult.Failed(IdentityErrors.Of(UserError.UsernameConflict));
		}

		return IdentityResult.Success;
	}
}