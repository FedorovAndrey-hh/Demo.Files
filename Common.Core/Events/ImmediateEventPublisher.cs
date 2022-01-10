using System.Collections.Immutable;
using System.Reflection;
using Common.Core.Diagnostics;

namespace Common.Core.Events;

public sealed class ImmediateEventPublisher : IEventPublisher
{
	public ImmediateEventPublisher(IImmutableList<object> handlers)
	{
		Preconditions.RequiresNotNull(handlers, nameof(handlers));
		Preconditions.RequiresNotNullItems(handlers, nameof(handlers));

		_handlers = handlers;
	}

	private readonly IImmutableList<object> _handlers;

	public void Publish(object @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var typedHandlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

		var exceptions = new List<Exception>();

		foreach (var handler in _handlers)
		{
			if (handler is IEventHandler genericHandler)
			{
				try
				{
					genericHandler.HandleEvent(@event);
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			foreach (var @interface in handler.GetType().GetInterfaces())
			{
				if (@interface.IsAssignableFrom(typedHandlerType))
				{
					var method = @interface.GetMethod(
						             nameof(IEventHandler<object>.HandleEvent),
						             @interface.GetGenericArguments()
					             )
					             ?? throw Errors.UnexpectedContract();

					try
					{
						method.Invoke(handler, new[] { @event });
					}
					catch (TargetInvocationException e)
					{
						exceptions.Add(e.InnerException ?? e);
					}
					catch (Exception e)
					{
						exceptions.Add(e);
					}
				}
			}
		}

		if (exceptions.Count > 0)
		{
			throw new EventPublicationException(exceptions);
		}
	}
}