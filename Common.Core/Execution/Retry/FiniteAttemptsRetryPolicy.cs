namespace Common.Core.Execution.Retry;

public sealed class FiniteAttemptsRetryPolicy : IRetryPolicy
{
	public FiniteAttemptsRetryPolicy(uint attemptsLimit)
	{
		AttemptsLimit = attemptsLimit;
	}

	public uint AttemptsLimit { get; }

	public IRetryScope CreateScope() => new Scope(AttemptsLimit);

	private sealed class Scope : IRetryScope
	{
		public Scope(uint attemptsLimit)
		{
			AttemptsLimit = attemptsLimit;
			CurrentRetryCount = 0;
		}

		public uint AttemptsLimit { get; }
		public uint CurrentRetryCount { get; private set; }

		public bool ShouldRetry(Exception exception)
		{
			var result = CurrentRetryCount < AttemptsLimit;

			CurrentRetryCount++;

			return result;
		}
	}
}