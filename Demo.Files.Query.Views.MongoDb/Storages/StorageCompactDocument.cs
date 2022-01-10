using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Files.Query.Views.MongoDb.Storages;

[BsonIgnoreExtraElements]
public sealed class StorageCompactDocument
{
	[BsonConstructor]
	public StorageCompactDocument(long storageId, ulong storageVersion, string? view)
	{
		StorageId = storageId;
		StorageVersion = storageVersion;
		View = view;
	}

	public long StorageId { get; }
	public ulong StorageVersion { get; }
	public string? View { get; }
}