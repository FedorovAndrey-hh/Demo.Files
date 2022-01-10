using Common.Core.Diagnostics;
using Common.Core.Events;
using Common.Core.Work;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain;

public sealed class AsUserRequestStorageResource : IAsUserRequestStorageResource
{
	public AsUserRequestStorageResource(
		User.IReadContext readContext,
		IAsyncWorkScopeProvider<IAuthorizationContext> workScopeProvider,
		IAsyncEventPublisher<UserEvent.Modified.ResourceRequested> publisher)
	{
		Preconditions.RequiresNotNull(readContext, nameof(readContext));
		Preconditions.RequiresNotNull(workScopeProvider, nameof(workScopeProvider));
		Preconditions.RequiresNotNull(publisher, nameof(publisher));

		_readContext = readContext;
		_workScopeProvider = workScopeProvider;
		_publisher = publisher;
	}

	private readonly User.IReadContext _readContext;
	private readonly IAsyncWorkScopeProvider<IAuthorizationContext> _workScopeProvider;
	private readonly IAsyncEventPublisher<UserEvent.Modified.ResourceRequested> _publisher;

	public Task<(User, IResourceRequestId)> ExecuteAsync(IUserId userId, UserVersion? userVersion)
	{
		Preconditions.RequiresNotNull(userId, nameof(userId));

		return _workScopeProvider.WithinScopeDoAsync(context => _ExecuteAsync(context.ForUser(), userId, userVersion));
	}

	private async Task<(User, IResourceRequestId)> _ExecuteAsync(
		User.IWriteContext context,
		IUserId userId,
		UserVersion? userVersion)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(userId, nameof(userId));

		var user = await User.GetAsync(_readContext, userId).ConfigureAwait(false);

		if (userVersion is not null)
		{
			user.AssertVersion(userVersion);
		}

		var resourceRequested = await user.RequestResourceAsync(context, ResourceType.Storage)
			.ConfigureAwait(false);

		await _publisher.PublishAsync(resourceRequested).ConfigureAwait(false);

		return (user.After(resourceRequested), resourceRequested.ResourceRequest.Id);
	}
}