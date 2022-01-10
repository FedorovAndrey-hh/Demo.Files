using Common.Core.Diagnostics;
using Common.Core.Events;
using Common.Core.Work;
using Demo.Files.FilesManagement.Applications.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Applications.Implementations.ByDomain;

public sealed class CreatePremiumStorage : ICreatePremiumStorage
{
	public CreatePremiumStorage(
		IAsyncWorkScopeProvider<IFilesManagementWriteContext> workScopeProvider,
		IAsyncEventPublisher<StorageEvent.Created> eventPublisher)
	{
		Preconditions.RequiresNotNull(workScopeProvider, nameof(workScopeProvider));
		Preconditions.RequiresNotNull(eventPublisher, nameof(eventPublisher));

		_workScopeProvider = workScopeProvider;
		_eventPublisher = eventPublisher;
	}

	private readonly IAsyncWorkScopeProvider<IFilesManagementWriteContext> _workScopeProvider;
	private readonly IAsyncEventPublisher<StorageEvent.Created> _eventPublisher;

	public Task<Storage> ExecuteAsync()
		=> _workScopeProvider.WithinScopeDoAsync(context => _ExecuteAsync(context.ForStorage()));

	private async Task<Storage> _ExecuteAsync(Storage.IWriteContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		var created = await Storage.CreateAsync(context, Limitations.Minimum).ConfigureAwait(false);

		await _eventPublisher.PublishAsync(created).ConfigureAwait(false);

		return Storage.After(created);
	}
}