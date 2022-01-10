using Common.Communication;
using Common.Core.Diagnostics;
using Demo.Files.Common.Contracts.Communication.FilesManagement;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Presentation.Common;

public sealed class InternalCommunicationEventPublisher : IInternalAsyncEventPublisher
{
	private readonly IMessageSender _messageSender;

	public InternalCommunicationEventPublisher(IMessageSender messageSender)
	{
		Preconditions.RequiresNotNull(messageSender, nameof(messageSender));

		_messageSender = messageSender;
	}

	public Task PublishAsync(StorageEvent.Created @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return _messageSender.SendMessageAsync(
			StorageCreatedMessage.Port,
			new StorageCreatedMessage(
				@event.Id.ToCommunicationContext(),
				@event.Version.ToCommunicationContext(),
				@event.Limitations.ToCommunicationContext()
			)
		);
	}

	public Task PublishAsync(StorageEvent.Modified.DirectoryAdded @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return _messageSender.SendMessageAsync(
			StorageDirectoryAddedMessage.Port,
			new StorageDirectoryAddedMessage(
				@event.Id.ToCommunicationContext(),
				@event.DirectoryId.ToCommunicationContext(),
				@event.DirectoryName.AsString(),
				@event.NewVersion.ToCommunicationContext()
			)
		);
	}

	public Task PublishAsync(StorageEvent.Modified.DirectoryRelocated @event)
	{
		// TODO: Implement PublishAsync.

		return Task.CompletedTask;
	}

	public Task PublishAsync(StorageEvent.Modified.DirectoryRemoved @event)
	{
		// TODO: Implement PublishAsync.

		return Task.CompletedTask;
	}

	public Task PublishAsync(StorageEvent.Modified.DirectoryRenamed @event)
	{
		// TODO: Implement PublishAsync.

		return Task.CompletedTask;
	}
}