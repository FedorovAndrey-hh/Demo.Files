using Common.Core.Diagnostics;

namespace Common.Core.Intervals;

public static class ComparerInterval
{
	public static ComparerInterval<T> FromInclusive<T>(T start, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));

		return new ComparerInterval<T>(IntervalLimit.Inclusive(start), null, comparer);
	}

	public static ComparerInterval<T> FromExclusive<T>(T start, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));

		return new ComparerInterval<T>(IntervalLimit.Exclusive(start), null, comparer);
	}

	public static ComparerInterval<T> ToInclusive<T>(T end, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(end, nameof(end));

		return new ComparerInterval<T>(null, IntervalLimit.Inclusive(end), comparer);
	}

	public static ComparerInterval<T> ToExclusive<T>(T end, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(end, nameof(end));

		return new ComparerInterval<T>(null, IntervalLimit.Exclusive(end), comparer);
	}

	public static ComparerInterval<T> Inclusive<T>(T start, T end, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));
		Preconditions.RequiresNotNull(end, nameof(end));

		return new ComparerInterval<T>(IntervalLimit.Inclusive(start), IntervalLimit.Inclusive(end), comparer);
	}

	public static ComparerInterval<T> Exclusive<T>(T start, T end, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));
		Preconditions.RequiresNotNull(end, nameof(end));

		return new ComparerInterval<T>(IntervalLimit.Exclusive(start), IntervalLimit.Exclusive(end), comparer);
	}

	public static ComparerInterval<T> InclusiveExclusive<T>(T start, T end, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));
		Preconditions.RequiresNotNull(end, nameof(end));

		return new ComparerInterval<T>(IntervalLimit.Inclusive(start), IntervalLimit.Exclusive(end), comparer);
	}

	public static ComparerInterval<T> ExclusiveInclusive<T>(T start, T end, IComparer<T>? comparer = null)
		where T : notnull
	{
		Preconditions.RequiresNotNull(start, nameof(start));
		Preconditions.RequiresNotNull(end, nameof(end));

		return new ComparerInterval<T>(IntervalLimit.Exclusive(start), IntervalLimit.Inclusive(end), comparer);
	}
}

public sealed class ComparerInterval<T> : IInterval<T>
	where T : notnull
{
	public ComparerInterval(IntervalLimit<T>? start, IntervalLimit<T>? end, IComparer<T>? comparer)
	{
		Comparer = comparer ?? Comparer<T>.Default;

		IntervalPreconditions.RequiresCorrectLimitsOrder(start, end, Comparer);

		Start = start;
		End = end;
	}

	public IComparer<T> Comparer { get; }

	public IntervalLimit<T>? Start { get; }

	public IntervalLimit<T>? End { get; }

	public IInterval<T> ChangeStart(IntervalLimit<T> value)
	{
		Preconditions.RequiresNotNull(value, nameof(value));

		return new ComparerInterval<T>(value, End, Comparer);
	}

	public IInterval<T> ChangeEnd(IntervalLimit<T> value)
	{
		Preconditions.RequiresNotNull(value, nameof(value));

		return new ComparerInterval<T>(Start, value, Comparer);
	}

	public bool Contains(T value)
	{
		Preconditions.RequiresNotNull(value, nameof(value));

		var comparer = Comparer;

		var start = Start;
		if (start is not null && start.IsGrater(value, comparer))
		{
			return false;
		}

		var end = End;
		if (end is not null && end.IsLess(value, comparer))
		{
			return false;
		}

		return true;
	}
}