using System.Collections;
using Common.Core.Diagnostics;

namespace Common.Core.Progressions;

public sealed class ProgressionEnumerator<TElement> : IEnumerator<TElement>
	where TElement : notnull
{
	public ProgressionEnumerator(IProgression<TElement> progression)
	{
		Preconditions.RequiresNotNull(progression, nameof(progression));

		_progression = progression;
	}

	private readonly IProgression<TElement> _progression;
	private bool _isInitialized = false;

	public bool MoveNext()
	{
		Current = _isInitialized ? _progression.Next(Current) : _progression.First;

		_isInitialized = true;

		return true;
	}

	public void Reset()
	{
		_isInitialized = false;
	}

	object IEnumerator.Current => Current;

	public void Dispose()
	{
	}

	public TElement Current { get; private set; } = default!;
}