using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

internal sealed class StorageDataUtility
{
	internal static void SetOriginalVersion(EntityEntry<StorageData> entry, StorageVersion version)
	{
		Preconditions.RequiresNotNull(entry, nameof(entry));

		entry.Property(e => e.Version).OriginalValue = version.Value;
	}
}