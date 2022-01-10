using Common.Core.Diagnostics;
using Common.Core.Execution;

namespace Common.Core.Modifications;

public static class ContinuousExtensions
{
	public static TThis AfterOptional<TThis, TEvent>(this TThis @this, TEvent? optionalEvent)
		where TThis : IContinuous<TThis, TEvent>
		where TEvent : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return optionalEvent is null ? @this : @this.After(optionalEvent);
	}

	public static TThis AfterMany<TThis, TEvent>(this TThis @this, params TEvent[] events)
		where TThis : IContinuous<TThis, TEvent>
		where TEvent : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(events, nameof(events));

		var result = @this;

		for (var index = 0; index < events.Length; index++)
		{
			result = result.After(@events[index]);
		}

		return result;
	}

	public static TThis AfterMany<TThis, TEvent>(this TThis @this, IEnumerable<TEvent> events)
		where TThis : IContinuous<TThis, TEvent>
		where TEvent : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(events, nameof(events));

		var result = @this;

		foreach (var @event in events)
		{
			result = result.After(@event);
		}

		return result;
	}

	public static TThis AfterAction<TThis, TEvent>(this TThis @this, Func<TThis, TEvent>? action)
		where TThis : IContinuous<TThis, TEvent>
		where TEvent : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (action is null)
		{
			return @this;
		}

		return @this.After(action(@this));
	}

	public static async Task<TThis> AfterActionAsync<TThis, TEvent>(
		this TThis @this,
		AsyncFunc<TThis, TEvent>? action)
		where TThis : IContinuous<TThis, TEvent>
		where TEvent : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (action is null)
		{
			return @this;
		}

		return @this.After(await action(@this).ConfigureAwait(false));
	}
}