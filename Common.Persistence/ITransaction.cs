namespace Common.Persistence;

public interface ITransaction : IAsyncDisposable
{
	public Task CommitAsync();

	public Task RollbackAsync();
}