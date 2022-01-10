using System.Data;
using Common.Core.Diagnostics;

namespace Common.Core.Intervals;

public static class IntervalPreconditions
{
	public static void RequiresCorrectLimitsOrder<T>(
		IntervalLimit<T>? start,
		IntervalLimit<T>? end,
		IComparer<T> comparer)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));
		Preconditions.RequiresNotNull(end, nameof(end));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		if (start is null || end is null)
		{
			return;
		}

		var correct = start.IsInclusive && end.IsInclusive
			? comparer.Compare(start.Value, end.Value) <= 0
			: comparer.Compare(start.Value, end.Value) < 0;
		if (!correct)
		{
			throw new ConstraintException("Start limit must not be grater than End limit.");
		}
	}
}