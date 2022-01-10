using Common.Core;
using Common.Core.Diagnostics;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public static class StorageExtensions
{
	public static void AssertVersion(this Storage @this, StorageVersion version)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(version, nameof(version));

		if (!Eq.ValueSafe(@this.Version, version))
		{
			throw new StorageException(StorageError.Outdated);
		}
	}
}