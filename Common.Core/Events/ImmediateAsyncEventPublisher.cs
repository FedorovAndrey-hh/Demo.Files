using System.Collections.Immutable;
using System.Reflection;
using Common.Core.Diagnostics;

namespace Common.Core.Events;

public sealed class ImmediateAsyncEventPublisher : IAsyncEventPublisher
{
	public ImmediateAsyncEventPublisher(IImmutableList<object> handlers)
	{
		Preconditions.RequiresNotNull(handlers, nameof(handlers));
		Preconditions.RequiresNotNullItems(handlers, nameof(handlers));

		_handlers = handlers;
	}

	private readonly IImmutableList<object> _handlers;

	public async Task PublishAsync(object @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		var exceptions = new List<Exception>();

		var genericHandlerType = typeof(IAsyncEventHandler<>).MakeGenericType(@event.GetType());

		foreach (var handler in _handlers)
		{
			if (handler is IAsyncEventHandler genericHandler)
			{
				try
				{
					await genericHandler.HandleEventAsync(@event).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					exceptions.Add(e);
				}
			}

			foreach (var @interface in handler.GetType().GetInterfaces())
			{
				if (@interface.IsAssignableFrom(genericHandlerType))
				{
					var method = @interface.GetMethod(
						             nameof(IAsyncEventHandler<object>.HandleEventAsync),
						             @interface.GetGenericArguments()
					             )
					             ?? throw Errors.UnexpectedContract();

					Task? actualTask;
					try
					{
						actualTask = (Task?)method.Invoke(handler, new[] { @event });
					}
					catch (TargetInvocationException e)
					{
						exceptions.Add(e.InnerException ?? e);
						continue;
					}
					catch (Exception e)
					{
						exceptions.Add(e);
						continue;
					}

					var expectedTask = actualTask ?? throw Errors.UnexpectedContract();

					try
					{
						await expectedTask.ConfigureAwait(false);
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