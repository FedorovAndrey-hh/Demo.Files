using Common.Core.Diagnostics;

namespace Common.Core.Execution.Decoration;

public sealed class WrapExceptionExecutionDecorator : IExecutionDecorator
{
	public WrapExceptionExecutionDecorator(IExceptionWrapper exceptionWrapper, bool wrapOnCapturedContext = true)
	{
		Preconditions.RequiresNotNull(exceptionWrapper, nameof(exceptionWrapper));

		ExceptionWrapper = exceptionWrapper;
		WrapOnCapturedContext = wrapOnCapturedContext;
	}

	public IExceptionWrapper ExceptionWrapper { get; }
	public bool WrapOnCapturedContext { get; }

	public void Decorate(Action block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			block();
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public void Decorate<TContext>(TContext context, Action<TContext> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			block(context);
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public async Task DecorateAsync(AsyncAction block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			await block().ConfigureAwait(WrapOnCapturedContext);
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public async Task DecorateAsync<TContext>(TContext context, AsyncAction<TContext> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			await block(context).ConfigureAwait(WrapOnCapturedContext);
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public TResult Decorate<TResult>(Func<TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			return block();
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public TResult Decorate<TContext, TResult>(TContext context, Func<TContext, TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			return block(context);
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public async Task<TResult> DecorateAsync<TResult>(AsyncFunc<TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			return await block().ConfigureAwait(WrapOnCapturedContext);
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}

	public async Task<TResult> DecorateAsync<TContext, TResult>(
		TContext context,
		AsyncFunc<TContext, TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		try
		{
			return await block(context).ConfigureAwait(WrapOnCapturedContext);
		}
		catch (Exception e) when (ExceptionWrapper.ShouldBeWrapped(e))
		{
			throw ExceptionWrapper.Wrap(e);
		}
	}
}