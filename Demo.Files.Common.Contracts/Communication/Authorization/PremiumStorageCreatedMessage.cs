using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Common.Contracts.Communication.Authorization;

public sealed class PremiumStorageCreatedMessage
{
	public const string Port = "auth-in:premium-storage-created";

	[JsonConstructor]
	public PremiumStorageCreatedMessage(string id, StorageId storageId)
	{
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(storageId, nameof(storageId));

		Id = id;
		StorageId = storageId;
	}

	public string Id { get; }
	public StorageId StorageId { get; }
}