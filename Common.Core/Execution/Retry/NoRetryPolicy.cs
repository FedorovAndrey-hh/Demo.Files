namespace Common.Core.Execution.Retry;

public sealed class NoRetryPolicy
	: IRetryPolicy,
	  IDelayedRetryPolicy,
	  IRetryScope,
	  IDelayedRetryScope
{
	private static NoRetryPolicy? _cache;
	public static NoRetryPolicy Create() => _cache ?? (_cache = new NoRetryPolicy());

	public static IRetryPolicy AsNormal() => Create();
	public static IDelayedRetryPolicy AsDelayed() => Create();

	private NoRetryPolicy()
	{
	}

	IRetryScope IRetryPolicy.CreateScope() => this;

	IDelayedRetryScope IDelayedRetryPolicy.CreateScope() => this;

	bool IRetryScope.ShouldRetry(Exception exception) => false;

	bool IDelayedRetryScope.ShouldRetry(Exception exception, out TimeSpan delay)
	{
		delay = TimeSpan.Zero;

		return false;
	}
}