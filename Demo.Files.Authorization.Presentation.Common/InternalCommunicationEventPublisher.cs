using Common.Communication;
using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Common.Contracts.Communication.Authorization;

namespace Demo.Files.Authorization.Presentation.Common;

public sealed class InternalCommunicationEventPublisher : IInternalAsyncEventPublisher
{
	private readonly IMessageSender _messageSender;

	public InternalCommunicationEventPublisher(IMessageSender messageSender)
	{
		Preconditions.RequiresNotNull(messageSender, nameof(messageSender));

		_messageSender = messageSender;
	}

	public async Task PublishAsync(UserEvent.Modified.ResourceRequested @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		await _messageSender.SendMessageAsync(
				RequestPremiumStorageMessage.Port,
				new RequestPremiumStorageMessage(
					CommunicationConverters.RequestPremiumStorageId.ConvertForward(
						(@event.Id, @event.ResourceRequest.Id)
					)
				)
			)
			.ConfigureAwait(false);
	}
}