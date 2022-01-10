namespace Common.Core.Events;

public interface IEventHandler
{
	public void HandleEvent(object @event);
}

public interface IEventHandler<in TEvent>
	where TEvent : notnull
{
	public void HandleEvent(TEvent @event);
}