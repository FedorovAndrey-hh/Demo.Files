using Common.Core.Diagnostics;

namespace Common.Core.Modifications;

public static class VersionExtensions
{
	public static bool IsIncrementOf<TVersion>(this TVersion @this, TVersion other)
		where TVersion : IVersion<TVersion>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));

		return other.CompareTo(@this) < 0 && Eq.ValueSafe(other.Increment(), @this);
	}

	public static bool IsDecrementOf<TVersion>(this TVersion @this, TVersion other)
		where TVersion : IVersion<TVersion>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(other, nameof(other));

		return other.CompareTo(@this) > 0 && Eq.ValueSafe(other.Decrement(), @this);
	}
}