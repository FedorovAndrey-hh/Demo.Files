namespace Common.Core.Execution.Retry;

public interface IRetryPolicy
{
	public IRetryScope CreateScope();
}