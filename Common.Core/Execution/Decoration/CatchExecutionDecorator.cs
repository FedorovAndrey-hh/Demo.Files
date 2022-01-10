using Common.Core.Diagnostics;

namespace Common.Core.Execution.Decoration;

public abstract class CatchExecutionDecorator : IExecutionDecorator
{
	protected static ActionResult PerformWrap(Func<Exception, Exception> wrap) => new ActionResult.Wrap(wrap);
	protected static ActionResult PerformRetry() => ActionResult.Retry.Create();
	protected static ActionResult PerformConsume() => ActionResult.Consume.Create();
	protected static ActionResult PerformRethrow() => ActionResult.Rethrow.Create();

	protected abstract class ActionResult
	{
		private ActionResult()
		{
		}

		public sealed class Wrap : ActionResult
		{
			internal Wrap(Func<Exception, Exception> wrap)
			{
				Preconditions.RequiresNotNull(wrap, nameof(wrap));

				_wrap = wrap;
			}

			private readonly Func<Exception, Exception> _wrap;

			public Exception WrapException(Exception exception)
			{
				Preconditions.RequiresNotNull(exception, nameof(exception));

				return _wrap(exception);
			}
		}

		public sealed class Retry : ActionResult
		{
			private static Retry? _cache;
			public static Retry Create() => _cache ?? (_cache = new Retry());

			private Retry()
			{
			}
		}

		public sealed class Consume : ActionResult
		{
			private static Consume? _cache;
			public static Consume Create() => _cache ?? (_cache = new Consume());

			private Consume()
			{
			}
		}

		public sealed class Rethrow : ActionResult
		{
			private static Rethrow? _cache;
			public static Rethrow Create() => _cache ?? (_cache = new Rethrow());

			private Rethrow()
			{
			}
		}
	}

	protected static Result<TResult> PerformWrap<TResult>(Func<Exception, Exception> wrap)
		=> new Result<TResult>.Wrap(wrap);

	protected static Result<TResult> PerformRetry<TResult>() => Result<TResult>.Retry.Create();

	protected static Result<TResult> PerformConsume<TResult>(TResult errorResult)
		=> new Result<TResult>.Consume(errorResult);

	protected static Result<TResult> PerformRethrow<TResult>() => Result<TResult>.Rethrow.Create();

	protected abstract class Result<TResult>
	{
		private Result()
		{
		}

		public sealed class Wrap : Result<TResult>
		{
			internal Wrap(Func<Exception, Exception> wrap)
			{
				Preconditions.RequiresNotNull(wrap, nameof(wrap));

				_wrap = wrap;
			}

			private readonly Func<Exception, Exception> _wrap;

			public Exception WrapException(Exception exception)
			{
				Preconditions.RequiresNotNull(exception, nameof(exception));

				return _wrap(exception);
			}
		}

		public sealed class Retry : Result<TResult>
		{
			private static Retry? _cache;
			public static Retry Create() => _cache ?? (_cache = new Retry());

			private Retry()
			{
			}
		}

		public sealed class Consume : Result<TResult>
		{
			internal Consume(TResult errorResult)
			{
				ErrorResult = errorResult;
			}

			public TResult ErrorResult { get; }
		}

		public sealed class Rethrow : Result<TResult>
		{
			private static Rethrow? _cache;
			public static Rethrow Create() => _cache ?? (_cache = new Rethrow());

			private Rethrow()
			{
			}
		}
	}

	protected abstract ActionResult OnActionException(Exception exception, int retryCount);

	protected abstract Result<TResult> OnException<TResult>(Exception exception, int retryCount);

	public void Decorate(Action block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				block();
				return;
			}
			catch (Exception e)
			{
				switch (OnActionException(e, retryCount++))
				{
					case ActionResult.Consume:
						return;
					case ActionResult.Rethrow:
						throw;
					case ActionResult.Retry:
						break;
					case ActionResult.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public void Decorate<TContext>(TContext context, Action<TContext> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				block(context);
				return;
			}
			catch (Exception e)
			{
				switch (OnActionException(e, retryCount++))
				{
					case ActionResult.Consume:
						return;
					case ActionResult.Rethrow:
						throw;
					case ActionResult.Retry:
						break;
					case ActionResult.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public async Task DecorateAsync(AsyncAction block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				await block();
				return;
			}
			catch (Exception e)
			{
				switch (OnActionException(e, retryCount++))
				{
					case ActionResult.Consume:
						return;
					case ActionResult.Rethrow:
						throw;
					case ActionResult.Retry:
						break;
					case ActionResult.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public async Task DecorateAsync<TContext>(TContext context, AsyncAction<TContext> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				await block(context);
				return;
			}
			catch (Exception e)
			{
				switch (OnActionException(e, retryCount++))
				{
					case ActionResult.Consume:
						return;
					case ActionResult.Rethrow:
						throw;
					case ActionResult.Retry:
						break;
					case ActionResult.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public TResult Decorate<TResult>(Func<TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				return block();
			}
			catch (Exception e)
			{
				switch (OnException<TResult>(e, retryCount++))
				{
					case Result<TResult>.Consume consume:
						return consume.ErrorResult;
					case Result<TResult>.Rethrow:
						throw;
					case Result<TResult>.Retry:
						break;
					case Result<TResult>.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public TResult Decorate<TContext, TResult>(TContext context, Func<TContext, TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				return block(context);
			}
			catch (Exception e)
			{
				switch (OnException<TResult>(e, retryCount++))
				{
					case Result<TResult>.Consume consume:
						return consume.ErrorResult;
					case Result<TResult>.Rethrow:
						throw;
					case Result<TResult>.Retry:
						break;
					case Result<TResult>.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public async Task<TResult> DecorateAsync<TResult>(AsyncFunc<TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				return await block();
			}
			catch (Exception e)
			{
				switch (OnException<TResult>(e, retryCount++))
				{
					case Result<TResult>.Consume consume:
						return consume.ErrorResult;
					case Result<TResult>.Rethrow:
						throw;
					case Result<TResult>.Retry:
						break;
					case Result<TResult>.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}

	public async Task<TResult> DecorateAsync<TContext, TResult>(
		TContext context,
		AsyncFunc<TContext, TResult> block)
	{
		Preconditions.RequiresNotNull(block, nameof(block));

		var retryCount = 0;
		while (true)
		{
			try
			{
				return await block(context);
			}
			catch (Exception e)
			{
				switch (OnException<TResult>(e, retryCount++))
				{
					case Result<TResult>.Consume consume:
						return consume.ErrorResult;
					case Result<TResult>.Rethrow:
						throw;
					case Result<TResult>.Retry:
						break;
					case Result<TResult>.Wrap wrap:
						throw wrap.WrapException(e);
					default:
						throw Contracts.UnreachableThrow();
				}
			}
		}
	}
}