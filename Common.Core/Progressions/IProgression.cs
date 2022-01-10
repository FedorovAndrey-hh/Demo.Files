namespace Common.Core.Progressions;

public interface IProgression<TElement> : IEnumerable<TElement>
	where TElement : notnull
{
	public TElement First { get; }

	public TElement Next(TElement element);
}