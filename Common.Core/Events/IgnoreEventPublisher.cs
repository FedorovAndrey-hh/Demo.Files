namespace Common.Core.Events;

public sealed class IgnoreEventPublisher
	: IEventPublisher,
	  IAsyncEventPublisher
{
	private static IgnoreEventPublisher? _cache;
	public static IgnoreEventPublisher Create() => _cache ?? (_cache = new IgnoreEventPublisher());

	private IgnoreEventPublisher()
	{
	}

	public void Publish(object @event)
	{
	}

	public Task PublishAsync(object @event) => Task.CompletedTask;
}