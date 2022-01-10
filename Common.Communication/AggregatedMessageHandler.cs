using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Communication;

public sealed class AggregatedMessageHandler<TMessage> : IMessageHandler<TMessage>
	where TMessage : notnull
{
	public AggregatedMessageHandler(IImmutableList<IMessageHandler<TMessage>> handlers)
	{
		Preconditions.RequiresNotNull(handlers, nameof(handlers));

		_handlers = handlers;
	}

	private readonly IImmutableList<IMessageHandler<TMessage>> _handlers;

	public async Task<HandleMessageResult> HandleMessageAsync(TMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		var result = HandleMessageResult.Success;

		foreach (var handler in _handlers)
		{
			result = result.Combine(await handler.HandleMessageAsync(message).ConfigureAwait(false));
		}

		return result;
	}
}