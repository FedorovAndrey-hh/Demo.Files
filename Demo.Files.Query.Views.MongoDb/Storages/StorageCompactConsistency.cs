using System.Text.Json;
using Common.Communication;
using Common.Core.Diagnostics;
using Common.Core.Modifications;
using Demo.Files.Common.Contracts.Communication;
using Demo.Files.Common.Contracts.Communication.FilesManagement;
using Demo.Files.Query.Views.Storages;
using MongoDB.Driver;

namespace Demo.Files.Query.Views.MongoDb.Storages;

public sealed class StorageCompactConsistency
	: IStorageCompactViewsConsistency
{
	public StorageCompactConsistency(
		IMongoCollection<StorageCompactDocument> collection,
		JsonSerializerOptions? viewJsonOptions)
	{
		Preconditions.RequiresNotNull(collection, nameof(collection));

		_collection = collection;
		_viewJsonOptions = viewJsonOptions;
	}

	private readonly IMongoCollection<StorageCompactDocument> _collection;
	private readonly JsonSerializerOptions? _viewJsonOptions;

	public async Task<HandleMessageResult> HandleMessageAsync(StorageCreatedMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		var rawStorageId = message.StorageId.Value;
		var rawStorageVersion = message.StorageVersion.Value;
		var document = await _collection
			.Find(
				Builders<StorageCompactDocument>
					.Filter
					.Eq(nameof(StorageCompactDocument.StorageId), rawStorageId)
			)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
		if (document is not null)
		{
			// Already created.
			return HandleMessageResult.Success;
		}

		await _collection
			.InsertOneAsync(
				new StorageCompactDocument(
					rawStorageId,
					rawStorageVersion,
					JsonSerializer.Serialize(
						new StorageCompact(
							id: rawStorageId,
							sizeInBytes: 0,
							directoriesCount: 0,
							filesCount: 0,
							limitations: new StorageCompact.StorageLimitations(
								message.Limitations.TotalSpaceInBytes,
								message.Limitations.TotalFileCount,
								message.Limitations.SingleFileSizeInBytes
							),
							version: rawStorageVersion
						),
						_viewJsonOptions
					)
				)
			)
			.ConfigureAwait(false);

		return HandleMessageResult.Success;
	}

	public Task<HandleMessageResult> HandleMessageAsync(StorageDirectoryAddedMessage message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		return _HandleUpdateMessageAsync(
			message.StorageId,
			message.StorageVersion,
			view =>
			{
				view.DirectoriesCount++;
				view.Version = message.StorageVersion.Value;
			}
		);
	}

	private async Task<HandleMessageResult> _HandleUpdateMessageAsync(
		StorageId storageId,
		StorageVersion storageVersion,
		Action<StorageCompact> update)
	{
		Preconditions.RequiresNotNull(storageId, nameof(storageId));
		Preconditions.RequiresNotNull(storageVersion, nameof(storageVersion));
		Preconditions.RequiresNotNull(update, nameof(update));

		var rawStorageId = storageId.Value;
		var rawStorageVersion = storageVersion.Value;
		var filter = Builders<StorageCompactDocument>
			.Filter
			.Eq(e => e.StorageId, rawStorageId);

		var document = await _collection
			.Find(filter)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
		if (document is null)
		{
			// Not created yet.
			return HandleMessageResult.CurrentlyUnprocessable;
		}

		if (!storageVersion.IsIncrementOf(StorageVersion.Of(document.StorageVersion)))
		{
			// Wrong message order.
			return HandleMessageResult.CurrentlyUnprocessable;
		}

		var documentView = document.View;
		var view = documentView is null
			? null
			: JsonSerializer.Deserialize<StorageCompact>(documentView, _viewJsonOptions);
		if (view is null)
		{
			return HandleMessageResult.Success;
		}

		update(view);

		var documentUpdate = Builders<StorageCompactDocument>.Update;
		await _collection
			.UpdateOneAsync(
				filter,
				documentUpdate.Combine(
					documentUpdate.Set(
						e => e.StorageVersion,
						rawStorageVersion
					),
					documentUpdate.Set(
						e => e.View,
						JsonSerializer.Serialize(view, _viewJsonOptions)
					)
				)
			)
			.ConfigureAwait(false);
		return HandleMessageResult.Success;
	}
}