using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core.Events;

public class MockAsyncEventHandler : ITestableAsyncEventHandler
{
	public async Task HandleEventAsync(object @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		await Task.Run(() => { Events = Events.Add(@event); });
	}

	public IImmutableList<object> Events { get; private set; } = ImmutableList.Create<object>();
}

public class MockAsyncEventHandler<TEvent> : ITestableAsyncEventHandler<TEvent>
	where TEvent : notnull
{
	public async Task HandleEventAsync(TEvent @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		await Task.Run(() => { Events = Events.Add(@event); });
	}

	public IImmutableList<TEvent> Events { get; private set; } = ImmutableList.Create<TEvent>();
}