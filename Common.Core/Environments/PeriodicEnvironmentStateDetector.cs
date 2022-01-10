namespace Common.Core.Environments;

public abstract class PeriodicEnvironmentStateDetector : IEnvironmentStateDetector
{
	protected PeriodicEnvironmentStateDetector(TimeSpan checkPeriod)
	{
		_checkPeriod = checkPeriod;
	}

	private readonly TimeSpan _checkPeriod;

	public async Task WaitReadyAsync(CancellationToken cancellationToken = default)
	{
		while (true)
		{
			cancellationToken.ThrowIfCancellationRequested();

			if (await CheckAsync().ConfigureAwait(false))
			{
				return;
			}

			await Task.Delay(_checkPeriod, cancellationToken).ConfigureAwait(false);
		}
	}

	protected abstract Task<bool> CheckAsync();
}