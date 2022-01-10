namespace Common.Core.Execution.Abstractions;

public interface IActionExecutor<in TAction>
	where TAction : notnull
{
	public void ExecuteAction(TAction action);
}