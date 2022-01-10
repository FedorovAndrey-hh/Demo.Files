namespace Common.Core.Execution.Decoration;

public interface IExecutionDecorator
	: IActionExecutionDecorator,
	  IFunctionExecutionDecorator,
	  IAsyncActionExecutionDecorator,
	  IAsyncFunctionExecutionDecorator
{
}