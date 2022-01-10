using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using static Demo.Files.Authorization.Applications.Abstractions.IAsUserGetRequestedStorageResource;

namespace Demo.Files.Authorization.Applications.Implementations.ByDomain;

public sealed class AsUserGetRequestedStorageResource : IAsUserGetRequestedStorageResource
{
	public AsUserGetRequestedStorageResource(User.IReadContext readContext)
	{
		Preconditions.RequiresNotNull(readContext, nameof(readContext));

		_readContext = readContext;
	}

	private readonly User.IReadContext _readContext;

	public async Task<Result> ExecuteAsync(IUserId userId, IResourceRequestId requestId)
	{
		Preconditions.RequiresNotNull(userId, nameof(userId));
		Preconditions.RequiresNotNull(requestId, nameof(requestId));

		var user = await User.GetAsync(_readContext, userId).ConfigureAwait(false);

		var resource = user.Resources.FirstOrDefault(
			e => e.Type == ResourceType.Storage && Eq.ValueSafe(e.RequestId, requestId)
		);

		if (resource is not null)
		{
			return new Result.Acquired(user, (Resource.Storage)resource);
		}

		if (user.ResourceRequests.Any(e => Eq.ValueSafe(e.Id, requestId)))
		{
			return new Result.Requested(user);
		}

		return new Result.NotFound(user);
	}
}