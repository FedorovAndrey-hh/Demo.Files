using Common.Core.Diagnostics;

namespace Common.Core;

public static class ComparerObjectExtensions
{
	public static bool EqualTo<T>(this T? @this, T? other, IEqualityComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Equals(@this, other);
	}

	public static bool NotEqualTo<T>(this T? @this, T? other, IEqualityComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return !comparer.Equals(@this, other);
	}

	public static bool EqualTo<T>(this T? @this, T? other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(@this, other) == 0;
	}

	public static bool NotEqualTo<T>(this T? @this, T? other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(@this, other) != 0;
	}

	public static bool LessThan<T>(this T? @this, T? other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(@this, other) < 0;
	}

	public static bool LessThanOrEqualTo<T>(this T? @this, T? other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(@this, other) <= 0;
	}

	public static bool GraterThan<T>(this T? @this, T? other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(@this, other) > 0;
	}

	public static bool GraterThanOrEqualTo<T>(this T? @this, T? other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(@this, other) >= 0;
	}
}