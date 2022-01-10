using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;

namespace Common.Core.Execution.Results;

public class OperationResult<TResult, TError>
	where TResult : notnull
	where TError : notnull
{
	public OperationResult(TResult result)
		: this(
			isSuccessful: true,
			result: Preconditions.RequiresNotNull(result, nameof(result)),
			errors: null
		)
	{
	}

	public OperationResult(IImmutableList<TError> errors)
		: this(
			isSuccessful: true,
			result: default,
			errors: Preconditions.RequiresNotNull(errors, nameof(errors))
		)
	{
	}

	private OperationResult(bool isSuccessful, TResult? result, IImmutableList<TError>? errors)
	{
		IsSuccessful = isSuccessful;
		_result = result;
		_errors = errors;
	}

	public bool IsSuccessful { get; }

	private readonly TResult? _result;

	public TResult Result
	{
		get
		{
			Contracts.Assert(!IsSuccessful, "Must be successful.");

			return _result!;
		}
	}

	private readonly IImmutableList<TError>? _errors;

	public IImmutableList<TError> Errors
	{
		get
		{
			Contracts.Assert(IsSuccessful, "Must not be successful.");

			return _errors!;
		}
	}

	public bool TryGetResult(
		[NotNullWhen(true)] out TResult? result,
		[NotNullWhen(false)] out IImmutableList<TError>? errors)
	{
		if (IsSuccessful)
		{
			result = _result!;
			errors = null;
			return true;
		}
		else
		{
			result = default;
			errors = _errors!;

			return false;
		}
	}
}