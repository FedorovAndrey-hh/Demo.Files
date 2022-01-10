using System.Diagnostics.CodeAnalysis;
using Common.Core;
using Common.Core.Modifications;

namespace Demo.Files.Common.Contracts.Communication;

public sealed class StorageVersion : IVersion<StorageVersion>
{
	public static StorageVersion Of(ulong value) => new(value);

	[return: NotNullIfNotNull("value")]
	public static StorageVersion? Of(ulong? value) => value.HasValue ? new StorageVersion(value.Value) : null;

	private StorageVersion(ulong value) => Value = value;

	public ulong Value { get; }

	public static bool operator ==(StorageVersion? lhs, StorageVersion? rhs) => Eq.ValueSafe(lhs, rhs);
	public static bool operator !=(StorageVersion? lhs, StorageVersion? rhs) => !Eq.ValueSafe(lhs, rhs);

	public bool Equals(StorageVersion? other) => other is not null && Eq.StructSafe(Value, other.Value);

	public override bool Equals(object? obj) => obj is StorageVersion other && Equals(other);

	public override int GetHashCode() => Value.GetHashCode();

	public int CompareTo(StorageVersion? other)
		=> ComparerUtility.CompareReferencesNullFirst(this, other, out var result)
			? result
			: Value.CompareTo(other.Value);

	public StorageVersion Increment() => new(checked(Value + 1));

	public StorageVersion Decrement() => new(checked(Value - 1));

	public override string ToString() => Value.ToString();
}