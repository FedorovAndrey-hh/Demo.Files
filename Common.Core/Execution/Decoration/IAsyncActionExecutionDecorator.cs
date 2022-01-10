namespace Common.Core.Execution.Decoration;

public interface IAsyncActionExecutionDecorator
{
	public Task DecorateAsync(AsyncAction block);

	public Task DecorateAsync<TContext>(TContext context, AsyncAction<TContext> block);
}