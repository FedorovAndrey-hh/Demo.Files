namespace Common.Core.Execution.Decoration;

public interface IExceptionWrapper
{
	public bool ShouldBeWrapped(Exception exception);

	public Exception Wrap(Exception exception);
}