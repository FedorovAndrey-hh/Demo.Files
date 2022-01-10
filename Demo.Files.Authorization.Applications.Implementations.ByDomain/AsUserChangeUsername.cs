using Common.Core.Diagnostics;
using Common.Core.Work;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain;

public sealed class AsUserChangeUsername : IAsUserChangeUsername
{
	public AsUserChangeUsername(
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

	public Task<User> ExecuteAsync(IUserId userId, UserVersion? userVersion, string newDisplayName)
	{
		Preconditions.RequiresNotNull(userId, nameof(userId));

		return _workScopeProvider.WithinScopeDoAsync(
			context => _ExecuteAsync(
				context.ForUser(),
				userId,
				userVersion,
				newDisplayName
			)
		);
	}

	private async Task<User> _ExecuteAsync(
		User.IWriteContext context,
		IUserId userId,
		UserVersion? userVersion,
		string newDisplayName)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(userId, nameof(userId));

		var user = await User.GetAsync(_readContext, userId).ConfigureAwait(false);

		if (userVersion is not null)
		{
			user.AssertVersion(userVersion);
		}

		return user.After(
			await user.ChangeUsernameAsync(context, UserDisplayName.Create(newDisplayName)).ConfigureAwait(false)
		);
	}
}