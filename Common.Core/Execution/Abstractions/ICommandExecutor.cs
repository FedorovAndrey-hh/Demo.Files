namespace Common.Core.Execution.Abstractions;

public interface ICommandExecutor<in TCommand, out TResult>
	where TCommand : notnull
{
	public TResult ExecuteCommand(TCommand command);
}