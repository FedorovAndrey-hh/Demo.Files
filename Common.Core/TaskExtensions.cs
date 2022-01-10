using System.Runtime.CompilerServices;
using Common.Core.Diagnostics;

namespace Common.Core;

public static class TaskExtensions
{
	public static void AsBlocking(this Task @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.GetAwaiter().GetResult();
	}

	public static void AsBlocking(this ValueTask @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.GetAwaiter().GetResult();
	}

	public static void AsBlocking(this ConfiguredTaskAwaitable @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.GetAwaiter().GetResult();
	}

	public static TResult AsBlocking<TResult>(this Task<TResult> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetAwaiter().GetResult();
	}

	public static TResult AsBlocking<TResult>(this ValueTask<TResult> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetAwaiter().GetResult();
	}

	public static TResult AsBlocking<TResult>(this ConfiguredTaskAwaitable<TResult> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetAwaiter().GetResult();
	}

	public static Task<Unit> UnitResult { get; } = Task.FromResult<Unit>(default);

	public static Task<bool> FalseResult { get; } = Task.FromResult(false);
	public static Task<bool> TrueResult { get; } = Task.FromResult(true);
}