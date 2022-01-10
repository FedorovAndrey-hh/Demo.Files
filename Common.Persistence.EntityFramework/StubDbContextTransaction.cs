using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Persistence.EntityFramework;

public class StubDbContextTransaction : IDbContextTransaction
{
	public StubDbContextTransaction()
		: this(Guid.NewGuid())
	{
	}

	public StubDbContextTransaction(Guid transactionId)
	{
		TransactionId = transactionId;
	}

	public Guid TransactionId { get; }

	public void Dispose()
	{
	}

	public ValueTask DisposeAsync() => ValueTask.CompletedTask;

	public void Commit()
	{
	}

	public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

	public void Rollback()
	{
	}

	public Task RollbackAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}