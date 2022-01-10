using System.Collections.Immutable;

namespace Common.Core.Events;

public interface ITestableAsyncEventHandler : IAsyncEventHandler
{
	public IImmutableList<object> Events { get; }
}

public interface ITestableAsyncEventHandler<TEvent> : IAsyncEventHandler<TEvent>
	where TEvent : notnull
{
	public IImmutableList<TEvent> Events { get; }
}