using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core.Events;

public class MockEventHandler : ITestableEventHandler
{
	public void HandleEvent(object @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		Events = Events.Add(@event);
	}

	public IImmutableList<object> Events { get; private set; } = ImmutableList.Create<object>();
}

public class MockEventHandler<TEvent> : ITestableEventHandler<TEvent>
	where TEvent : notnull
{
	public void HandleEvent(TEvent @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		Events = Events.Add(@event);
	}

	public IImmutableList<TEvent> Events { get; private set; } = ImmutableList.Create<TEvent>();
}