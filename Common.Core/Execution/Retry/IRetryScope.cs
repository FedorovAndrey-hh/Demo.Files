namespace Common.Core.Execution.Retry;

public interface IRetryScope
{
	public bool ShouldRetry(Exception exception);
}