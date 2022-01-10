using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public static class ResourceRequestDataExtensions
{
	public static ResourceRequestId GetId(this ResourceRequestData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return ResourceRequestId.Of(@this.Id);
	}

	public static ResourceType GetResourceType(this ResourceRequestData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Enum.TryParse<ResourceType>(@this.Type, true, out var result)
			? result
			: throw new ContractException($"Invalid {nameof(ResourceType)} format: {@this.Type}.");
	}

	public static void SetResourceType(this ResourceRequestData @this, ResourceType type)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Type = Enum.GetName(type)!;
	}

	public static UserId GetOwnerId(this ResourceRequestData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return UserId.Of(@this.OwnerId);
	}

	public static void SetOwnerId(this ResourceRequestData @this, UserId ownerId)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(ownerId, nameof(ownerId));

		@this.OwnerId = ownerId.Value;
	}
}