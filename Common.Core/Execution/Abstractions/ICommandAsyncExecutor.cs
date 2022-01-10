namespace Common.Core.Execution.Abstractions;

public interface ICommandAsyncExecutor<in TCommand, TResult>
	where TCommand : notnull
{
	public Task<TResult> ExecuteCommandAsync(TCommand command);
}