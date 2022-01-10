namespace Common.Core.Execution.Abstractions;

public interface IActionAsyncExecutor<in TAction>
	where TAction : notnull
{
	public Task ExecuteActionAsync(TAction action);
}