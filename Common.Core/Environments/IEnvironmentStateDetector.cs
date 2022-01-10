namespace Common.Core.Environments;

public interface IEnvironmentStateDetector
{
	public Task WaitReadyAsync(CancellationToken cancellationToken = default);
}