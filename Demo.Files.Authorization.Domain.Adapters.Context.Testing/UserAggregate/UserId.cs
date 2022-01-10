using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.Testing.UserAggregate;

public sealed record UserId : IUserId
{
	public static UserId Of(long value) => new(value);

	private UserId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IUserId? other) => other is UserId otherTyped && Equals(otherTyped);
}