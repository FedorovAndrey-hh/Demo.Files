namespace Common.Core;

public readonly struct Unit : IEquatable<Unit>
{
	public bool Equals(Unit other) => true;

	public override bool Equals(object? obj) => obj is Unit;

	public override int GetHashCode() => GetType().GetHashCode();

	public override string ToString() => nameof(Unit);
}