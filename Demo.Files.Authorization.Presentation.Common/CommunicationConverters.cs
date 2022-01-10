using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

namespace Demo.Files.Authorization.Presentation.Common;

public static class CommunicationConverters
{
	public static IBiConverter<(IUserId, IResourceRequestId), string> RequestPremiumStorageId { get; }
		= new RequestPremiumStorageIdConverter();

	private sealed class RequestPremiumStorageIdConverter
		: IBiConverter<(IUserId, IResourceRequestId), string>,
		  IConverter<(IUserId, IResourceRequestId), string>,
		  IConverter<string, (IUserId, IResourceRequestId)>

	{
		public IConverter<(IUserId, IResourceRequestId), string> Forward => this;
		public IConverter<string, (IUserId, IResourceRequestId)> Backward => this;

		string IConverter<(IUserId, IResourceRequestId), string>.Convert((IUserId, IResourceRequestId) source)
		{
			var (userId, resourceRequestId) = source;
			return string.Join(':', userId.RawLong().ToString(), resourceRequestId.RawLong().ToString());
		}

		(IUserId, IResourceRequestId) IConverter<string, (IUserId, IResourceRequestId)>.Convert(string source)
		{
			Preconditions.RequiresNotNull(source, nameof(source));

			var parts = source.Split(':', 3);
			Contracts.Assert(parts.Length == 2, "Invalid message id.");

			return (UserId.Of(long.Parse(parts[0])), ResourceRequestId.Of(long.Parse(parts[1])));
		}
	}
}