namespace Common.Core.Events;

public interface IAsyncEventPublisher
{
	public Task PublishAsync(object @event);
}

public interface IAsyncEventPublisher<in TEvent>
	where TEvent : notnull
{
	public Task PublishAsync(TEvent @event);
}