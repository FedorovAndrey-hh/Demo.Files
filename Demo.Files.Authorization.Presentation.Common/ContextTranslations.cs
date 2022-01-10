using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

namespace Demo.Files.Authorization.Presentation.Common;

using CommunicationStorageId = Files.Common.Contracts.Communication.StorageId;

public static class ContextTranslations
{
	public static StorageId ToAuthorizationContext(this CommunicationStorageId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return StorageId.Of(@this.Value);
	}

	public static CommunicationStorageId ToCommunicationContext(this IStorageId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return CommunicationStorageId.Of(@this.RawLong());
	}
}