using Common.Core.Diagnostics;
using Common.Core.Events;
using Common.Core.Work;
using Demo.Files.FilesManagement.Applications.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Implementations.ByDomain;

public sealed class AddDirectory : IAddDirectory
{
	public AddDirectory(
		Storage.IReadContext readContext,
		IAsyncWorkScopeProvider<IFilesManagementWriteContext> workScopeProvider,
		IAsyncEventPublisher<StorageEvent.Modified.DirectoryAdded> eventPublisher)
	{
		Preconditions.RequiresNotNull(readContext, nameof(readContext));
		Preconditions.RequiresNotNull(workScopeProvider, nameof(workScopeProvider));
		Preconditions.RequiresNotNull(eventPublisher, nameof(eventPublisher));

		_readContext = readContext;
		_workScopeProvider = workScopeProvider;
		_eventPublisher = eventPublisher;
	}

	private readonly Storage.IReadContext _readContext;
	private readonly IAsyncWorkScopeProvider<IFilesManagementWriteContext> _workScopeProvider;
	private readonly IAsyncEventPublisher<StorageEvent.Modified.DirectoryAdded> _eventPublisher;

	public Task<(Storage, IDirectoryId)> ExecuteAsync(
		IStorageId storageId,
		StorageVersion? storageVersion,
		string directoryName)
	{
		Preconditions.RequiresNotNull(storageId, nameof(storageId));

		return _workScopeProvider.WithinScopeDoAsync(
			context => _ExecuteAsync(
				context.ForStorage(),
				storageId,
				storageVersion,
				directoryName
			)
		);
	}

	private async Task<(Storage, IDirectoryId)> _ExecuteAsync(
		Storage.IWriteContext context,
		IStorageId storageId,
		StorageVersion? storageVersion,
		string directoryName)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(storageId, nameof(storageId));

		var storage = await Storage.GetAsync(_readContext, storageId).ConfigureAwait(false);

		if (storageVersion is not null)
		{
			storage.AssertVersion(storageVersion);
		}

		var directoryAdded = await storage
			.AddDirectoryAsync(context, DirectoryName.Create(directoryName))
			.ConfigureAwait(false);

		await _eventPublisher.PublishAsync(directoryAdded).ConfigureAwait(false);

		return (storage.After(directoryAdded), directoryAdded.DirectoryId);
	}
}