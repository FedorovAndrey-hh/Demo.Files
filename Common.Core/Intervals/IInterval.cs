namespace Common.Core.Intervals;

public interface IInterval<T>
	where T : notnull
{
	public IntervalLimit<T>? Start { get; }

	public IntervalLimit<T>? End { get; }

	public bool Contains(T value);

	public IInterval<T> ChangeStart(IntervalLimit<T> value);

	public IInterval<T> ChangeEnd(IntervalLimit<T> value);
}