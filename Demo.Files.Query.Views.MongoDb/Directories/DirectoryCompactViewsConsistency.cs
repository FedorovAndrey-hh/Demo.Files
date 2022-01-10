using System.Text.Json;
using Common.Communication;
using Common.Core.Diagnostics;
using Demo.Files.Common.Contracts.Communication.FilesManagement;
using Demo.Files.Query.Views.Directories;
using MongoDB.Driver;

namespace Demo.Files.Query.Views.MongoDb.Directories;

public sealed class DirectoryCompactViewsConsistency : IDirectoryCompactViewsConsistency
{
	public DirectoryCompactViewsConsistency(
		IMongoCollection<DirectoryCompactDocument> collection,
		JsonSerializerOptions? viewJsonOptions)
	{
		Preconditions.RequiresNotNull(collection, nameof(collection));

		_collection = collection;
		_viewJsonOptions = viewJsonOptions;
	}

	private readonly IMongoCollection<DirectoryCompactDocument> _collection;
	private readonly JsonSerializerOptions? _viewJsonOptions;

	public async Task<HandleMessageResult> HandleMessageAsync(StorageDirectoryAddedMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		var storageId = message.StorageId.Value;
		var directoryId = message.DirectoryId.Value;
		var rawStorageVersion = message.StorageVersion.Value;

		var filter = Builders<DirectoryCompactDocument>.Filter;

		var documentExists = await _collection
			.Find(
				filter.And(
					filter.Eq(e => e.StorageId, storageId),
					filter.Eq(e => e.DirectoryId, directoryId)
				)
			)
			.AnyAsync()
			.ConfigureAwait(false);

		if (documentExists)
		{
			// Already created.
			return HandleMessageResult.Success;
		}

		await _collection.BulkWriteAsync(
				new WriteModel<DirectoryCompactDocument>[]
				{
					new UpdateManyModel<DirectoryCompactDocument>(
						filter.Eq(e => e.StorageId, storageId),
						Builders<DirectoryCompactDocument>.Update.Set(
							e => e.StorageVersion,
							rawStorageVersion
						)
					),
					new InsertOneModel<DirectoryCompactDocument>(
						new DirectoryCompactDocument(
							storageId,
							rawStorageVersion,
							directoryId,
							JsonSerializer.Serialize(
								new DirectoryCompact(
									id: directoryId,
									name: message.DirectoryName,
									sizeInBytes: 0,
									filesCount: 0,
									storageId: storageId
								),
								_viewJsonOptions
							)
						)
					)
				}
			)
			.ConfigureAwait(false);
		
		return HandleMessageResult.Success;
	}
}