namespace Common.Core.Execution.Retry;

public interface IDelayedRetryScope
{
	public bool ShouldRetry(Exception exception, out TimeSpan delay);
}