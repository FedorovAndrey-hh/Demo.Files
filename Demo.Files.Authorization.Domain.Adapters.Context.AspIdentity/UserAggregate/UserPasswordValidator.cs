using Common.Core;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public sealed class UserPasswordValidator : IPasswordValidator<UserData>
{
	private static UserPasswordValidator? _cache;
	public static UserPasswordValidator Create() => _cache ?? (_cache = new UserPasswordValidator());

	private UserPasswordValidator()
	{
	}

	public Task<IdentityResult> ValidateAsync(UserManager<UserData> manager, UserData user, string password)
	{
		if (Password.Validator.Validate(password, out var error))
		{
			return IdentityResult.Success.AsTaskResult();
		}
		else
		{
			return IdentityResult.Failed(IdentityErrors.Of(error)).AsTaskResult();
		}
	}
}