using Common.Core.Diagnostics;
using Common.Core.Modifications;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed record ResourceRequest : IIdentifiable<IResourceRequestId>
{
	public static ResourceRequest Create(IResourceRequestId id, ResourceType type)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		return new ResourceRequest(id, type);
	}

	private ResourceRequest(IResourceRequestId id, ResourceType type)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		Id = id;
		Type = type;
	}

	public IResourceRequestId Id { get; }
	public ResourceType Type { get; }
}