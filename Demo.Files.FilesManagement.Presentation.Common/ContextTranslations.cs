using Common.Core.Data;
using Common.Core.Diagnostics;
using Demo.Files.Common.Contracts.Communication;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;
using StorageId = Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate.StorageId;
using StorageVersion = Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate.StorageVersion;
using CommunicationStorageId = Demo.Files.Common.Contracts.Communication.StorageId;
using CommunicationStorageVersion = Demo.Files.Common.Contracts.Communication.StorageVersion;
using CommunicationDirectoryId = Demo.Files.Common.Contracts.Communication.DirectoryId;

namespace Demo.Files.FilesManagement.Presentation.Common;

public static class ContextTranslations
{
	public static StorageId ToFilesManagementContext(this CommunicationStorageId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return StorageId.Of(@this.Value);
	}

	public static CommunicationStorageId ToCommunicationContext(this IStorageId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return CommunicationStorageId.Of(@this.RawLong());
	}

	public static StorageVersion ToFilesManagementContext(this CommunicationStorageVersion @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return StorageVersion.Of(@this.Value);
	}

	public static CommunicationStorageVersion ToCommunicationContext(this StorageVersion @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return CommunicationStorageVersion.Of(@this.Value);
	}

	public static CommunicationDirectoryId ToCommunicationContext(this IDirectoryId @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return CommunicationDirectoryId.Of(@this.RawLong());
	}

	public static StorageLimitations ToCommunicationContext(this Limitations @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return new StorageLimitations(
			@this.TotalSpace.MeasureInBytes(),
			@this.TotalFileCount,
			@this.SingleFileSize.MeasureInBytes()
		);
	}
}