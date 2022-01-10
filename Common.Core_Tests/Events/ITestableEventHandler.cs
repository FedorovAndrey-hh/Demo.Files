using System.Collections.Immutable;

namespace Common.Core.Events;

public interface ITestableEventHandler : IEventHandler
{
	public IImmutableList<object> Events { get; }
}

public interface ITestableEventHandler<TEvent> : IEventHandler<TEvent>
	where TEvent : notnull
{
	public IImmutableList<TEvent> Events { get; }
}