using Common.Core.Diagnostics;

namespace Common.Core.Execution.Decoration;

public sealed class NoDecorationExecutionDecorator : IExecutionDecorator
{
	private static NoDecorationExecutionDecorator? _cache;
	public static NoDecorationExecutionDecorator Create() => _cache ?? (_cache = new NoDecorationExecutionDecorator());

	private NoDecorationExecutionDecorator()
	{
	}

	public void Decorate(Action block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		block();
	}

	public void Decorate<TContext>(TContext context, Action<TContext> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		block(context);
	}

	public Task DecorateAsync(AsyncAction block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		return block();
	}

	public Task DecorateAsync<TContext>(TContext context, AsyncAction<TContext> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		return block(context);
	}

	public TResult Decorate<TResult>(Func<TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		return block();
	}

	public TResult Decorate<TContext, TResult>(TContext context, Func<TContext, TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		return block(context);
	}

	public Task<TResult> DecorateAsync<TResult>(AsyncFunc<TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		return block();
	}

	public Task<TResult> DecorateAsync<TContext, TResult>(TContext context, AsyncFunc<TContext, TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		return block(context);
	}
}