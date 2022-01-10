using System.Diagnostics.CodeAnalysis;
using Common.Core;
using Common.Core.Modifications;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed class UserVersion : IVersion<UserVersion>
{
	public static UserVersion Initial { get; } = new(0);

	public static UserVersion Of(ulong value) => new(value);

	[return: NotNullIfNotNull("value")]
	public static UserVersion? Of(ulong? value) => value.HasValue ? new UserVersion(value.Value) : null;

	private UserVersion(ulong value) => Value = value;

	public ulong Value { get; }

	public static bool operator ==(UserVersion? lhs, UserVersion? rhs) => Eq.ValueSafe(lhs, rhs);
	public static bool operator !=(UserVersion? lhs, UserVersion? rhs) => !Eq.ValueSafe(lhs, rhs);

	public bool Equals(UserVersion? other) => other is not null && Eq.StructSafe(Value, other.Value);

	public override bool Equals(object? obj) => obj is UserVersion other && Equals(other);

	public override int GetHashCode() => Value.GetHashCode();

	public int CompareTo(UserVersion? other)
		=> ComparerUtility.CompareReferencesNullFirst(this, other, out var result)
			? result
			: Value.CompareTo(other.Value);

	public UserVersion Increment() => new(checked(Value + 1));

	public UserVersion Decrement() => new(checked(Value - 1));

	public override string ToString() => Value.ToString();
}