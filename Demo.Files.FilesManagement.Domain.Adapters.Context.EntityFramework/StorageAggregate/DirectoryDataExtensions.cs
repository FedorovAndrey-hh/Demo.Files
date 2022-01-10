using Common.Core.Diagnostics;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

public static class DirectoryDataExtensions
{
	public static void ResetId(this DirectoryData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Id = default;
	}

	public static void SetId(this DirectoryData @this, DirectoryId id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Id = id.Value;
	}

	public static DirectoryId GetId(this DirectoryData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DirectoryId.Of(@this.Id);
	}

	public static void SetName(this DirectoryData @this, DirectoryName name)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(name, nameof(name));

		@this.Name = name.AsString();
	}

	public static DirectoryName GetName(this DirectoryData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return DirectoryName.Create(@this.Name);
	}

	public static void SetStorageId(this DirectoryData @this, StorageId id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.StorageId = id.Value;
	}

	public static StorageId GetStorageId(this DirectoryData @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return StorageId.Of(@this.StorageId);
	}
}