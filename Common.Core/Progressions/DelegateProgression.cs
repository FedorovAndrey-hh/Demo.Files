using System.Collections;
using Common.Core.Diagnostics;

namespace Common.Core.Progressions;

public sealed class DelegateProgression<TElement> : IProgression<TElement>
	where TElement : notnull
{
	public DelegateProgression(TElement first, Func<TElement, TElement> next)
	{
		Preconditions.RequiresNotNull(first, nameof(first));
		Preconditions.RequiresNotNull(next, nameof(next));

		_next = next;
		First = first;
	}

	public TElement First { get; }

	private readonly Func<TElement, TElement> _next;

	public TElement Next(TElement element) => _next(element);

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<TElement> GetEnumerator() => new ProgressionEnumerator<TElement>(this);
}