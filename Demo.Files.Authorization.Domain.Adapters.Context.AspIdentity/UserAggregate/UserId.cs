using System.Diagnostics.CodeAnalysis;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public sealed record UserId : IUserId
{
	public static UserId Of(long value) => new(value);

	[return: NotNullIfNotNull("value")]
	public static UserId? Of(long? value) => value.HasValue ? new UserId(value.Value) : null;

	private UserId(long value) => Value = value;

	public long Value { get; }

	public bool Equals(IUserId? other) => other is UserId otherTyped && Equals(otherTyped);
}