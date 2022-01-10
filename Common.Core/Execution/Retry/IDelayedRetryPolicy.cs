namespace Common.Core.Execution.Retry;

public interface IDelayedRetryPolicy
{
	public IDelayedRetryScope CreateScope();
}