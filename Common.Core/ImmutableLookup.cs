using System.Collections;
using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core;

public sealed class ImmutableLookup
{
	public ImmutableLookup<TKey, TElement> Empty<TKey, TElement>()
		where TKey : notnull
		=> new(ImmutableDictionary<TKey, IImmutableGrouping<TKey, TElement>>.Empty);

	private ImmutableLookup()
	{
	}

	public static ImmutableLookup This { get; } = new();
}

public sealed class ImmutableLookup<TKey, TElement> : IImmutableLookup<TKey, TElement>
	where TKey : notnull
{
	internal ImmutableLookup(IImmutableDictionary<TKey, IImmutableGrouping<TKey, TElement>> groups)
	{
		Preconditions.RequiresNotNull(groups, nameof(groups));

		_groups = groups;
	}

	private IImmutableDictionary<TKey, IImmutableGrouping<TKey, TElement>> _groups;

	public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator() => _groups.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public bool Contains(TKey key) => _groups.ContainsKey(key);

	public int Count => _groups.Count;

	public IEnumerable<TElement> this[TKey key]
		=> _groups.TryGetValue(key, out var grouping) ? grouping : Enumerable.Empty<TElement>();

	IImmutableList<TElement> IImmutableLookup<TKey, TElement>.this[TKey key]
		=> _groups.TryGetValue(key, out var grouping) ? grouping.Elements : ImmutableList<TElement>.Empty;

	public IImmutableLookup<TKey, TElement> Add(TKey key, TElement element)
		=> new ImmutableLookup<TKey, TElement>(
			_groups.SetItem(
				key,
				_groups.TryGetValue(key, out var grouping)
					? grouping.Add(element)
					: new ImmutableGrouping<TKey, TElement>(key, ImmutableList.Create<TElement>(element))
			)
		);

	public IImmutableLookup<TKey, TElement> Remove(TKey key, TElement element)
		=> _groups.TryGetValue(key, out var elements)
			? new ImmutableLookup<TKey, TElement>(_groups.SetItem(key, elements.Remove(element)))
			: this;
}