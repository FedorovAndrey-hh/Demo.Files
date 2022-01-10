using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Files.Query.Views.MongoDb.Directories;

[BsonIgnoreExtraElements]
public sealed class DirectoryCompactDocument
{
	[BsonConstructor]
	public DirectoryCompactDocument(long storageId, ulong storageVersion, long directoryId, string? view)
	{
		StorageId = storageId;
		StorageVersion = storageVersion;
		DirectoryId = directoryId;
		View = view;
	}

	public long StorageId { get; set; }
	public ulong StorageVersion { get; set; }
	public long DirectoryId { get; set; }
	public string? View { get; set; }
}