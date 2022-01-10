using System.Collections.Immutable;

namespace Common.Core;

public interface IImmutableLookup<TKey, TElement> : ILookup<TKey, TElement>
	where TKey : notnull
{
	public new IImmutableList<TElement> this[TKey key] { get; }

	public IImmutableLookup<TKey, TElement> Add(TKey key, TElement element);

	public IImmutableLookup<TKey, TElement> Remove(TKey key, TElement element);
}