using System.Collections.Immutable;

namespace Common.Core;

public interface IImmutableGrouping<out TKey, TElement> : IGrouping<TKey, TElement>
{
	public IImmutableList<TElement> Elements { get; }

	public IImmutableGrouping<TKey, TElement> Add(TElement element);

	public IImmutableGrouping<TKey, TElement> Remove(TElement element);
}