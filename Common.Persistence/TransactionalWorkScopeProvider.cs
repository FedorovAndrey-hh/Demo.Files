using Common.Core.Diagnostics;
using Common.Core.Execution;
using Common.Core.Work;

namespace Common.Persistence;

public abstract class TransactionalWorkScopeProvider<TScope> : IAsyncWorkScopeProvider<TScope>
	where TScope : notnull
{
	protected abstract Task<ITransaction> BeginTransactionAsync();

	protected abstract TScope ScopeOf(ITransaction transaction);

	public async Task<TResult> WithinScopeDoAsync<TResult>(
		AsyncFunc<TScope, TResult> action,
		bool executeOnCapturedContext = false)
	{
		Preconditions.RequiresNotNull(action, nameof(action));

		var transaction = await BeginTransactionAsync().ConfigureAwait(executeOnCapturedContext);
		try
		{
			var result = await action(ScopeOf(transaction)).ConfigureAwait(false);
			await transaction.CommitAsync().ConfigureAwait(false);
			return result;
		}
		catch
		{
			await transaction.RollbackAsync().ConfigureAwait(false);
			throw;
		}
	}

	public async Task WithinScopeDoAsync(
		AsyncAction<TScope> action,
		bool executeOnCapturedContext = false)
	{
		Preconditions.RequiresNotNull(action, nameof(action));

		var transaction = await BeginTransactionAsync().ConfigureAwait(executeOnCapturedContext);
		try
		{
			await action(ScopeOf(transaction)).ConfigureAwait(false);
			await transaction.CommitAsync().ConfigureAwait(false);
		}
		catch
		{
			await transaction.RollbackAsync().ConfigureAwait(false);
			throw;
		}
	}
}