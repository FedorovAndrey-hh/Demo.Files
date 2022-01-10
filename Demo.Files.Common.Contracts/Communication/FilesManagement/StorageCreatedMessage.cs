using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Common.Contracts.Communication.FilesManagement;

public sealed class StorageCreatedMessage
{
	public const string Port = "fm-out:storage-created";

	[JsonConstructor]
	public StorageCreatedMessage(StorageId storageId, StorageVersion storageVersion, StorageLimitations limitations)
	{
		Preconditions.RequiresNotNull(storageId, nameof(storageId));
		Preconditions.RequiresNotNull(storageVersion, nameof(storageVersion));
		Preconditions.RequiresNotNull(limitations, nameof(limitations));

		StorageId = storageId;
		StorageVersion = storageVersion;
		Limitations = limitations;
	}

	public StorageId StorageId { get; }
	public StorageVersion StorageVersion { get; }

	public StorageLimitations Limitations { get; }
}