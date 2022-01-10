namespace Common.Core.Execution.Decoration;

public interface IAsyncFunctionExecutionDecorator
{
	public Task<TResult> DecorateAsync<TResult>(AsyncFunc<TResult> block);

	public Task<TResult> DecorateAsync<TContext, TResult>(TContext context, AsyncFunc<TContext, TResult> block);
}