using Common.Core.Diagnostics;
using Common.Persistence;
using Demo.Files.FilesManagement.Domain.Abstractions;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

public sealed class FilesManagementWorkScopeProvider : TransactionalWorkScopeProvider<IFilesManagementWriteContext>
{
	public FilesManagementWorkScopeProvider(IFilesManagementPersistenceContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		_context = context;
	}

	private readonly IFilesManagementPersistenceContext _context;

	protected override async Task<ITransaction> BeginTransactionAsync()
		=> await _context.BeginTransactionAsync().ConfigureAwait(false);

	protected override IFilesManagementWriteContext ScopeOf(ITransaction transaction)
	{
		Preconditions.RequiresNotNull(transaction, nameof(transaction));

		return (IFilesManagementTransactionalWriteContext)transaction;
	}
}