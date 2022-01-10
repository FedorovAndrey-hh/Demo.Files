using Common.Core.Data;
using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public static class StorageDataExtensions
{
	public static void SetId(this StorageData @this, StorageId id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Id = id.Value;
	}

	public static StorageId GetId(this StorageData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return StorageId.Of(@this.Id);
	}

	public static void SetVersion(this StorageData @this, StorageVersion version)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Version = version.Value;
	}

	public static StorageVersion GetVersion(this StorageData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return StorageVersion.Of(@this.Version);
	}

	public static void SetLimitations(this StorageData @this, Limitations limitations)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		@this.LimitationsTotalSpace = limitations.TotalSpace.MeasureInBytes();
		@this.LimitationsTotalFileCount = limitations.TotalFileCount;
		@this.LimitationsSingleFileSize = limitations.SingleFileSize.MeasureInBytes();
	}

	public static Limitations GetLimitations(this StorageData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return Limitations.Create(
			@this.LimitationsTotalSpace.Bytes(),
			@this.LimitationsTotalFileCount,
			@this.LimitationsSingleFileSize.Bytes()
		);
	}
}