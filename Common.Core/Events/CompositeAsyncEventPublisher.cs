using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core.Events;

public abstract class CompositeAsyncEventPublisher : IAsyncEventPublisher
{
	protected CompositeAsyncEventPublisher(IImmutableList<IAsyncEventPublisher> publishers)
	{
		Preconditions.RequiresNotNull(publishers, nameof(publishers));

		_publishers = publishers;
	}

	private readonly IImmutableList<IAsyncEventPublisher> _publishers;

	protected abstract bool ShouldBePublishedBy(IAsyncEventPublisher publisher, object @event);

	public async Task PublishAsync(object @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var exceptions = new List<Exception>();

		foreach (var publisher in _publishers)
		{
			if (ShouldBePublishedBy(publisher, @event))
			{
				try
				{
					await publisher.PublishAsync(@event).ConfigureAwait(false);
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