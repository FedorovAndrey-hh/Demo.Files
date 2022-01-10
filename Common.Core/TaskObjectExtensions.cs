namespace Common.Core;

public static class TaskObjectExtensions
{
	public static Task<T> AsTaskResult<T>(this T @this) => Task.FromResult(@this);

	public static Task<bool> AsTaskResult(this bool @this)
		=> @this ? TaskExtensions.TrueResult : TaskExtensions.FalseResult;
}