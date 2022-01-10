using Common.Core.Diagnostics;

namespace Demo.Files.Common.Contracts;

public class AsyncOperationResponse<TResult, TError>
	where TResult : notnull
	where TError : notnull
{
	public AsyncOperationResponse()
		: this(
			status: AsyncOperationStatus.InProgress,
			result: default,
			error: default
		)
	{
	}

	public AsyncOperationResponse(TResult result)
		: this(
			status: AsyncOperationStatus.CompletedSuccessfully,
			result: result,
			error: default
		)
	{
	}

	public AsyncOperationResponse(TError error)
		: this(
			status: AsyncOperationStatus.CompletedUnsuccessfully,
			result: default,
			error: Preconditions.RequiresNotNull(error, nameof(error))
		)
	{
	}

	private AsyncOperationResponse(AsyncOperationStatus status, TResult? result, TError? error)
	{
		Status = status;
		Result = result;
		Error = error;
	}

	public AsyncOperationStatus Status { get; }

	public TResult? Result { get; }
	public TError? Error { get; }
}