using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core.Events;

public abstract class CompositeEventPublisher : IEventPublisher
{
	protected CompositeEventPublisher(IImmutableList<IEventPublisher> publishers)
	{
		Preconditions.RequiresNotNull(publishers, nameof(publishers));

		_publishers = publishers;
	}

	private readonly IImmutableList<IEventPublisher> _publishers;

	protected abstract bool ShouldBePublishedBy(IEventPublisher publisher, object @event);

	public void Publish(object @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var exceptions = new List<Exception>();

		foreach (var publisher in _publishers)
		{
			if (ShouldBePublishedBy(publisher, @event))
			{
				try
				{
					publisher.Publish(@event);
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}
		}

		if (exceptions.Count > 0)
		{
			throw new EventPublicationException(exceptions);
		}
	}
}