using Common.Core.Diagnostics;
using Common.Core.Events;
using Common.Core.Work;
using Demo.Files.FilesManagement.Applications.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Implementations.ByDomain;

public sealed class RenameDirectory : IRenameDirectory
{
	public RenameDirectory(
		Storage.IReadContext readContext,
		IAsyncWorkScopeProvider<IFilesManagementWriteContext> workScopeProvider,
		IAsyncEventPublisher<StorageEvent.Modified.DirectoryRenamed> eventPublisher)
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
	private readonly IAsyncEventPublisher<StorageEvent.Modified.DirectoryRenamed> _eventPublisher;

	public Task<Storage> ExecuteAsync(
		IStorageId storageId,
		StorageVersion? storageVersion,
		IDirectoryId directoryId,
		string newDirectoryName)
	{
		Preconditions.RequiresNotNull(storageId, nameof(storageId));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

		return _workScopeProvider.WithinScopeDoAsync(
			context => _ExecuteAsync(
				context.ForStorage(),
				storageId,
				storageVersion,
				directoryId,
				newDirectoryName
			)
		);
	}

	private async Task<Storage> _ExecuteAsync(
		Storage.IWriteContext context,
		IStorageId storageId,
		StorageVersion? storageVersion,
		IDirectoryId directoryId,
		string newDirectoryName)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(storageId, nameof(storageId));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));

		var storage = await Storage.GetAsync(_readContext, storageId).ConfigureAwait(false);
		if (storageVersion is not null)
		{
			storage.AssertVersion(storageVersion);
		}

		var directoryRenamed = await storage
			.RenameDirectoryAsync(context, directoryId, DirectoryName.Create(newDirectoryName))
			.ConfigureAwait(false);

		if (directoryRenamed is not null)
		{
			await _eventPublisher.PublishAsync(directoryRenamed).ConfigureAwait(false);
		}

		return storage.After(directoryRenamed);
	}
}