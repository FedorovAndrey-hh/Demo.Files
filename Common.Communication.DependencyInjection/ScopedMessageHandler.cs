using Common.Core.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Communication.DependencyInjection;

public sealed class ScopedMessageHandler<TMessage> : IMessageHandler<TMessage>
	where TMessage : notnull
{
	public ScopedMessageHandler(IServiceProvider services, bool allowMultiple)
	{
		Preconditions.RequiresNotNull(services, nameof(services));

		_services = services;
		_allowMultiple = allowMultiple;
	}

	private readonly IServiceProvider _services;
	private readonly bool _allowMultiple;

	public async Task<HandleMessageResult> HandleMessageAsync(TMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		await using (var scope = _services.CreateAsyncScope())
		{
			if (_allowMultiple)
			{
				var handlers = scope.ServiceProvider.GetServices<IMessageHandler<TMessage>>();

				var result = HandleMessageResult.Success;

				foreach (var handler in handlers)
				{
					result = result.Combine(await handler.HandleMessageAsync(message).ConfigureAwait(false));
				}

				return result;
			}
			else
			{
				var handler = scope.ServiceProvider.GetRequiredService<IMessageHandler<TMessage>>();
				return await handler.HandleMessageAsync(message).ConfigureAwait(false);
			}
		}
	}
}