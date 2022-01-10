using Common.Core.Diagnostics;

namespace Common.Core.Intervals;

public static class IntervalLimit
{
	public static IntervalLimit<T> Inclusive<T>(T value)
		where T : notnull
		=> new(true, value);

	public static IntervalLimit<T> Exclusive<T>(T value)
		where T : notnull
		=> new(false, value);
}

public sealed class IntervalLimit<T>
	where T : notnull
{
	public IntervalLimit(bool isInclusive, T value)
	{
		Preconditions.RequiresNotNull(value, nameof(value));

		IsInclusive = isInclusive;
		Value = value;
	}

	public bool IsInclusive { get; }
	public T Value { get; }

	public bool IsEqual(T other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return IsInclusive ? comparer.Compare(Value, other) == 0 : false;
	}

	public bool IsLess(T other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(Value, other) < 0;
	}

	public bool IsLessOrEqual(T other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return IsLess(other, comparer) || IsEqual(other, comparer);
	}

	public bool IsGrater(T other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return comparer.Compare(Value, other) > 0;
	}

	public bool IsGraterOrEqual(T other, IComparer<T> comparer)
	{
		Preconditions.RequiresNotNull(other, nameof(other));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return IsGrater(other, comparer) || IsEqual(other, comparer);
	}
}