using System.Collections;
using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core;

public sealed class ImmutableGrouping<TKey, TElement> : IImmutableGrouping<TKey, TElement>
	where TKey : notnull
{
	public ImmutableGrouping(TKey key, IImmutableList<TElement> elements)
	{
		Preconditions.RequiresNotNull(elements, nameof(elements));
		Preconditions.RequiresNotNull(key, nameof(key));

		Elements = elements;
		Key = key;
	}

	public IImmutableList<TElement> Elements { get; }

	public IEnumerator<TElement> GetEnumerator() => Elements.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public TKey Key { get; }

	public IImmutableGrouping<TKey, TElement> Add(TElement element)
		=> new ImmutableGrouping<TKey, TElement>(Key, Elements.Add(element));

	public IImmutableGrouping<TKey, TElement> Remove(TElement element)
		=> new ImmutableGrouping<TKey, TElement>(Key, Elements.Remove(element));
}