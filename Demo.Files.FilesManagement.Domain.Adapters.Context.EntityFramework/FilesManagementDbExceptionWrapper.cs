using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Common.EntityFramework;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework;

internal sealed class FilesManagementDbExceptionWrapper : IExceptionWrapper
{
	private static FilesManagementDbExceptionWrapper? _cache;

	internal static FilesManagementDbExceptionWrapper Create()
		=> _cache ?? (_cache = new FilesManagementDbExceptionWrapper());

	private FilesManagementDbExceptionWrapper()
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

		return new StorageException(StorageError.Outdated, innerException: exception);
	}
}