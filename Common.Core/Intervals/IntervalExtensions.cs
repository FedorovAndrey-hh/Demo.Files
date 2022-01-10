using Common.Core.Diagnostics;

namespace Common.Core.Intervals;

public static class IntervalExtensions
{
	public static string FormatInMathNotation<T>(this IInterval<T> @this)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return new IntervalMathFormatter<T>(null).Format(@this);
	}
}