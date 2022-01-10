using System.Text.Json;
using Common.Core.Diagnostics;
using Common.Core.Text;
using Demo.Files.Query.Views.Directories;
using MongoDB.Driver;

namespace Demo.Files.Query.Views.MongoDb.Directories;

public sealed class DirectoryCompactViews
	: IDirectoryCompactViews
{
	public DirectoryCompactViews(
		IMongoCollection<DirectoryCompactDocument> collection,
		JsonSerializerOptions? viewJsonOptions)
	{
		Preconditions.RequiresNotNull(collection, nameof(collection));

		_collection = collection;
		_viewJsonOptions = viewJsonOptions;
	}

	private readonly IMongoCollection<DirectoryCompactDocument> _collection;
	private readonly JsonSerializerOptions? _viewJsonOptions;

	public async Task<string> FindAllByStorageIdAsync(long storageId)
	{
		var filter = Builders<DirectoryCompactDocument>.Filter;
		var views = await _collection
			.Find(
				filter.And(
					filter.Eq(e => e.StorageId, storageId),
					filter.Not(filter.Eq(e => e.View, null))
				)
			)
			.Project(document => document.View)
			.ToListAsync()
			.ConfigureAwait(false);

		return JsonSerializer.Serialize(new JsonRawEnumerable(views!), _viewJsonOptions);
	}

	public Task<string?> FindByIdAsync(long storageId, long directoryId)
	{
		var filter = Builders<DirectoryCompactDocument>.Filter;
		return _collection
			.Find(
				filter.And(
					filter.Eq(e => e.StorageId, storageId),
					filter.Eq(e => e.DirectoryId, directoryId)
				)
			)
			.Project(document => document.View)
			.FirstOrDefaultAsync();
	}
}