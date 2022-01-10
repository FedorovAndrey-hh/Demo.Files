using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;
using Microsoft.EntityFrameworkCore.Storage;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

internal sealed class FilesManagementTransactionalWriteContext : IFilesManagementTransactionalWriteContext
{
	internal FilesManagementTransactionalWriteContext(
		FilesManagementDbContext dbContext,
		IDbContextTransaction dbContextTransaction,
		IExceptionWrapper exceptionWrapper)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(dbContextTransaction, nameof(dbContextTransaction));
		Preconditions.RequiresNotNull(exceptionWrapper, nameof(exceptionWrapper));

		_dbContext = dbContext;
		_dbContextTransaction = dbContextTransaction;
		_exceptionWrapper = exceptionWrapper;
	}

	private readonly FilesManagementDbContext _dbContext;
	private readonly IDbContextTransaction _dbContextTransaction;
	private readonly IExceptionWrapper _exceptionWrapper;

	public ValueTask DisposeAsync() => _dbContextTransaction.DisposeAsync();

	public async Task CommitAsync()
	{
		try
		{
			await _dbContextTransaction.CommitAsync().ConfigureAwait(false);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	public Task RollbackAsync() => _dbContextTransaction.RollbackAsync();

	public Storage.IWriteContext ForStorage() => new StorageWriteContext(_dbContext, _exceptionWrapper);
}