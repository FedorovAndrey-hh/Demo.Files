using Common.Core.Diagnostics;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain.AspIdentity;

public sealed class AsGuestSignIn : IAsGuestSignIn
{
	public AsGuestSignIn(User.IReadContext readContext, UserManager<UserData> userManager)
	{
		Preconditions.RequiresNotNull(readContext, nameof(readContext));
		Preconditions.RequiresNotNull(userManager, nameof(userManager));

		_readContext = readContext;
		_userManager = userManager;
	}

	private readonly User.IReadContext _readContext;
	private readonly UserManager<UserData> _userManager;

	public async Task<IAsGuestSignIn.Result> ExecuteAsync(string emailOrUsername, string password)
	{
		UserData? userData = null;
		User? user = null;
		if (UserEmail.Parser.Parse(emailOrUsername, out var email, out _))
		{
			userData = await _userManager.FindByEmailAsync(emailOrUsername).ConfigureAwait(false);
			user = await User.FindAsync(_readContext, email).ConfigureAwait(false);

			if (userData is null || user is null)
			{
				return new IAsGuestSignIn.Result.Fail(IAsGuestSignIn.Result.Error.NotFound);
			}
		}

		if (Username.Parser.Parse(emailOrUsername, out var username, out _))
		{
			userData = await _userManager.FindByNameAsync(emailOrUsername).ConfigureAwait(false);
			user = await User.FindAsync(_readContext, username).ConfigureAwait(false);

			if (userData is null || user is null)
			{
				return new IAsGuestSignIn.Result.Fail(IAsGuestSignIn.Result.Error.NotFound);
			}
		}

		if (userData is null || user is null)
		{
			return new IAsGuestSignIn.Result.Fail(IAsGuestSignIn.Result.Error.InvalidIdentity);
		}

		if (await _userManager.CheckPasswordAsync(userData, password))
		{
			return new IAsGuestSignIn.Result.Success(user);
		}
		else
		{
			return new IAsGuestSignIn.Result.Fail(IAsGuestSignIn.Result.Error.InvalidPassword);
		}
	}
}