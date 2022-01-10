using Common.Core.Diagnostics;
using Common.Core.Execution.Retry;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Presentations.Internal;

public sealed class MessageHandlerRetryPolicy : IDelayedRetryPolicy
{
	public IDelayedRetryScope CreateScope() => new Scope();

	private sealed class Scope : IDelayedRetryScope
	{
		private int _retryCount = 0;

		public bool ShouldRetry(Exception exception, out TimeSpan delay)
		{
			Preconditions.RequiresNotNull(exception, nameof(exception));

			_retryCount++;

			delay = TimeSpan.FromMilliseconds(500 * Math.Min(_retryCount, 10));

			return exception is StorageException storageException && storageException.Error == StorageError.Outdated;
		}
	}
}