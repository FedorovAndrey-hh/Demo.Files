using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Common.EntityFramework;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

internal sealed class AuthorizationDbExceptionWrapper : IExceptionWrapper
{
	private static AuthorizationDbExceptionWrapper? _cache;

	internal static AuthorizationDbExceptionWrapper Create()
		=> _cache ?? (_cache = new AuthorizationDbExceptionWrapper());

	private AuthorizationDbExceptionWrapper()
	{
	}

	public bool ShouldBeWrapped(Exception exception)
	{
		Preconditions.RequiresNotNull(exception, nameof(exception));

		return exception.IsEntityFrameworkConcurrencyException();
	}

	public Exception Wrap(Exception exception)
	{
		Preconditions.RequiresNotNull(exception, nameof(exception));

		throw new UserException(UserError.Outdated, innerException: exception);
	}
}