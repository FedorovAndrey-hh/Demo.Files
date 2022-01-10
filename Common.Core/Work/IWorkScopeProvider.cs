namespace Common.Core.Work;

public interface IWorkScopeProvider<out TScope>
	where TScope : notnull
{
	public TResult WithinScopeDo<TResult>(Func<TScope, TResult> action);

	public Task WithinScopeDo(Action<TScope> action);
}