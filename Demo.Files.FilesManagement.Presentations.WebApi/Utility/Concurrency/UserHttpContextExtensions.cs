using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Presentations.WebApi.Utility.Concurrency;

public static class StorageHttpContextExtensions
{
	public static void SetResponseStorageVersion(this HttpContext @this, StorageVersion version)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(version, nameof(version));

		@this.SetETagSingleValue(version.Value.ToString());
	}
}