using Common.Core.Diagnostics;
using Common.Core.Work;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain;

public sealed class AsUserChangePassword : IAsUserChangePassword
{
	public AsUserChangePassword(
		User.IReadContext readContext,
		IAsyncWorkScopeProvider<IAuthorizationContext> workScopeProvider)
	{
		Preconditions.RequiresNotNull(readContext, nameof(readContext));
		Preconditions.RequiresNotNull(workScopeProvider, nameof(workScopeProvider));

		_readContext = readContext;
		_workScopeProvider = workScopeProvider;
	}

	private readonly User.IReadContext _readContext;
	private readonly IAsyncWorkScopeProvider<IAuthorizationContext> _workScopeProvider;

	public Task<User> ExecuteAsync(IUserId userId, UserVersion? userVersion, string currentPassword, string newPassword)
	{
		Preconditions.RequiresNotNull(userId, nameof(userId));

		return _workScopeProvider.WithinScopeDoAsync(
			context => _ExecuteAsync(
				context.ForUser(),
				userId,
				userVersion,
				currentPassword,
				newPassword
			)
		);
	}

	private async Task<User> _ExecuteAsync(
		User.IWriteContext context,
		IUserId userId,
		UserVersion? userVersion,
		string currentPassword,
		string newPassword)
	{
		Preconditions.RequiresNotNull(userId, nameof(userId));

		var user = await User.GetAsync(_readContext, userId).ConfigureAwait(false);

		if (userVersion is not null)
		{
			user.AssertVersion(userVersion);
		}

		var passwordChanged = await user.ChangePasswordAsync(
				context,
				Password.Create(currentPassword),
				Password.Create(newPassword)
			)
			.ConfigureAwait(false);

		return user.After(passwordChanged);
	}
}