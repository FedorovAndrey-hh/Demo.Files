using Common.Core.Diagnostics;
using Common.Core.Work;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain;

public sealed class AsUserAcquireStorageResource : IAsUserAcquireStorageResource
{
	public AsUserAcquireStorageResource(
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

	public Task<User> ExecuteAsync(IUserId userId, UserVersion? userVersion, Resource.Storage resource)
	{
		Preconditions.RequiresNotNull(userId, nameof(userId));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		return _workScopeProvider.WithinScopeDoAsync(
			context => _ExecuteAsync(
				context.ForUser(),
				userId,
				userVersion,
				resource
			)
		);
	}

	private async Task<User> _ExecuteAsync(
		User.IWriteContext context,
		IUserId userId,
		UserVersion? userVersion,
		Resource.Storage resource)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(userId, nameof(userId));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		var user = await User.GetAsync(_readContext, userId).ConfigureAwait(false);

		if (userVersion is not null)
		{
			user.AssertVersion(userVersion);
		}

		var resourceAcquired = await user.AcquireResourceAsync(context, resource).ConfigureAwait(false);

		return user.After(resourceAcquired);
	}
}