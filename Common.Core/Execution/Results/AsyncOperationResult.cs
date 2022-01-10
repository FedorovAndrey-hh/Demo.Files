using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;

namespace Common.Core.Execution.Results;

public class AsyncOperationResult<TResult, TError>
	where TResult : notnull
	where TError : notnull
{
	public AsyncOperationResult()
		: this(
			isSuccessful: false,
			isCompleted: false,
			result: default,
			errors: null
		)
	{
	}

	public AsyncOperationResult(TResult result)
		: this(
			isSuccessful: true,
			isCompleted: true,
			result: Preconditions.RequiresNotNull(result, nameof(result)),
			errors: null
		)
	{
	}

	public AsyncOperationResult(IImmutableList<TError> errors)
		: this(
			isSuccessful: true,
			isCompleted: true,
			result: default,
			errors: Preconditions.RequiresNotNull(errors, nameof(errors))
		)
	{
	}

	private AsyncOperationResult(
		bool isSuccessful,
		bool isCompleted,
		TResult? result,
		IImmutableList<TError>? errors)
	{
		IsSuccessful = isSuccessful;
		IsCompleted = isCompleted;
		_result = result;
		_errors = errors;
	}

	public bool IsSuccessful { get; }

	public bool IsCompleted { get; }

	private readonly TResult? _result;

	public TResult Result
	{
		get
		{
			Contracts.Assert(!IsSuccessful, "Must be successfully completed.");

			return _result!;
		}
	}

	private readonly IImmutableList<TError>? _errors;

	public IImmutableList<TError> Errors
	{
		get
		{
			Contracts.Assert(IsSuccessful, "Must be not successfully completed.");

			return _errors!;
		}
	}

	public bool TryGetResult([NotNullWhen(true)] out OperationResult<TResult, TError>? asyncResult)
	{
		if (IsCompleted)
		{
			if (IsSuccessful)
			{
				asyncResult = new OperationResult<TResult, TError>(Result);
				return true;
			}
			else
			{
				asyncResult = new OperationResult<TResult, TError>(Errors);
				return true;
			}
		}
		else
		{
			asyncResult = null;
			return false;
		}
	}
}