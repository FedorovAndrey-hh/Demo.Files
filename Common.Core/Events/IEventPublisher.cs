namespace Common.Core.Events;

public interface IEventPublisher
{
	public void Publish(object @event);
}

public interface IEventPublisher<in TEvent>
	where TEvent : notnull
{
	public void Publish(TEvent @event);
}