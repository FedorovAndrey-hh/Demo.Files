using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Common.Contracts.Communication.FilesManagement;

public sealed class StorageDirectoryAddedMessage
{
	public const string Port = "fm-out:storage-directory-added";

	[JsonConstructor]
	public StorageDirectoryAddedMessage(
		StorageId storageId,
		DirectoryId directoryId,
		string directoryName,
		StorageVersion storageVersion)
	{
		Preconditions.RequiresNotNull(storageId, nameof(storageId));
		Preconditions.RequiresNotNull(directoryId, nameof(directoryId));
		Preconditions.RequiresNotNull(directoryName, nameof(directoryName));
		Preconditions.RequiresNotNull(storageVersion, nameof(storageVersion));

		StorageId = storageId;
		DirectoryId = directoryId;
		DirectoryName = directoryName;
		StorageVersion = storageVersion;
	}

	public StorageId StorageId { get; }
	public DirectoryId DirectoryId { get; }
	public string DirectoryName { get; }
	public StorageVersion StorageVersion { get; }
}