using Common.Core.Execution;

namespace Common.Core.Work;

public interface IAsyncWorkScopeProvider<out TScope>
	where TScope : notnull
{
	public Task<TResult> WithinScopeDoAsync<TResult>(
		AsyncFunc<TScope, TResult> action,
		bool executeOnCapturedContext = false);

	public Task WithinScopeDoAsync(
		AsyncAction<TScope> action,
		bool executeOnCapturedContext = false);
}