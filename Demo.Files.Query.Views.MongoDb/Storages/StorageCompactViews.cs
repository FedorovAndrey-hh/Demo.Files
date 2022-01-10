using Common.Core.Diagnostics;
using Demo.Files.Query.Views.Storages;
using MongoDB.Driver;

namespace Demo.Files.Query.Views.MongoDb.Storages;

public sealed class StorageCompactViews
	: IStorageCompactViews
{
	public StorageCompactViews(IMongoCollection<StorageCompactDocument> collection)
	{
		Preconditions.RequiresNotNull(collection, nameof(collection));

		_collection = collection;
	}

	private readonly IMongoCollection<StorageCompactDocument> _collection;

	public Task<string?> FindByIdAsync(long id)
		=> _collection
			.Find(Builders<StorageCompactDocument>.Filter.Eq(e => e.StorageId, id))
			.Project(document => document.View)
			.FirstOrDefaultAsync();
}