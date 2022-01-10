namespace Common.Core.Execution.Decoration;

public interface IActionExecutionDecorator
{
	public void Decorate(Action block);

	public void Decorate<TContext>(TContext context, Action<TContext> block);
}