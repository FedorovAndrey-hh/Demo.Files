using Common.Core.Diagnostics;
using Common.Core.Execution.Retry;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Presentations.Internal;

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

			return exception is UserException userException && userException.Error == UserError.Outdated;
		}
	}
}