namespace Common.Core.Execution.Decoration;

public interface IFunctionExecutionDecorator
{
	public TResult Decorate<TResult>(Func<TResult> block);

	public TResult Decorate<TContext, TResult>(TContext context, Func<TContext, TResult> block);
}