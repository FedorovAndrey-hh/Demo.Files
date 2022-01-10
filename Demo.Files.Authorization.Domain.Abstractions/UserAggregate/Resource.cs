using Common.Core;
using Common.Core.Diagnostics;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public abstract record Resource : ITypeEnum<Resource, Resource.Storage>
{
	private Resource(ResourceType type, IResourceRequestId requestId)
	{
		Preconditions.RequiresNotNull(requestId, nameof(requestId));

		Type = type;
		RequestId = requestId;
	}

	public ResourceType Type { get; }

	public IResourceRequestId RequestId { get; }

	public sealed record Storage : Resource
	{
		public Storage(IStorageId id, IResourceRequestId requestId)
			: base(ResourceType.Storage, requestId)
		{
			Preconditions.RequiresNotNull(id, nameof(id));

			Id = id;
		}

		public IStorageId Id { get; }
	}
}