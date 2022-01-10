using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public static class IdentityErrors
{
	public static IdentityError Of(UserError error)
	{
		var identityError = new IdentityError();
		identityError.Code = Enum.GetName(error);
		identityError.Description = Enum.GetName(error);
		return identityError;
	}
}