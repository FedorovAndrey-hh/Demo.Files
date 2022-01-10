namespace Common.Core;

public static class TimeSpanTaskExtensions
{
	public static Task ToDelayTask(this TimeSpan @this, CancellationToken cancellationToken = default)
		=> Eq.StructSafe(@this, TimeSpan.Zero) ? Task.CompletedTask : Task.Delay(@this, cancellationToken);

	public static Task ToDelayTask(this TimeSpan? @this, CancellationToken cancellationToken = default)
		=> @this.HasValue ? @this.Value.ToDelayTask(cancellationToken) : Task.CompletedTask;
}