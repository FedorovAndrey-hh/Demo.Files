using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public sealed record ResourceRequestId : IResourceRequestId
{
	public static ResourceRequestId Of(long value) => new(value);

	private ResourceRequestId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IResourceRequestId? other) => other is ResourceRequestId otherTyped && Equals(otherTyped);
}