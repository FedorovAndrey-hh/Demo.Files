namespace Common.Core.Events;

public interface IAsyncEventHandler
{
	public Task HandleEventAsync(object @event);
}

public interface IAsyncEventHandler<in TEvent>
	where TEvent : notnull
{
	public Task HandleEventAsync(TEvent @event);
}