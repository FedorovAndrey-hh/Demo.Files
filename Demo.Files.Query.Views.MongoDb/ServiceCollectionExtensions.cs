using System.Text.Json;
using Common.Core.Diagnostics;
using Common.Core.Text;
using Common.DependencyInjection;
using Demo.Files.Query.Views.Directories;
using Demo.Files.Query.Views.Files;
using Demo.Files.Query.Views.MongoDb.Directories;
using Demo.Files.Query.Views.MongoDb.Files;
using Demo.Files.Query.Views.MongoDb.Storages;
using Demo.Files.Query.Views.MongoDb.Users;
using Demo.Files.Query.Views.Storages;
using Demo.Files.Query.Views.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Demo.Files.Query.Views.MongoDb;

public static class ServiceCollectionExtensions
{
	public static void AddMongoDbFilesViews(
		this IServiceCollection @this,
		IConfiguration? configuration = null,
		Action<MongoDbFilesViewsOptions>? configure = null)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (configuration is not null)
		{
			@this.Configure<MongoDbFilesViewsOptions>(configuration);
		}

		@this.PostConfigure<MongoDbFilesViewsOptions>(
			options =>
			{
				if (configure is not null)
				{
					configure(options);
				}

				if (options.Json is null)
				{
					options.Json = new JsonSerializerOptions();
				}

				options.Json.Converters.Add(new JsonRawEnumerable.Converter());

				if (options.Settings == null)
				{
					options.Settings = new MongoClientSettings();
				}

				options.Settings.Credential = MongoCredential.CreateCredential(
					options.Database,
					options.Username,
					options.Password
				);

				if (options.Host is not null)
				{
					if (options.Port.HasValue)
					{
						options.Settings.Server = new MongoServerAddress(options.Host, options.Port.Value);
					}
					else
					{
						options.Settings.Server = new MongoServerAddress(options.Host);
					}
				}
			}
		);

		@this.AddScoped<IDirectoryCompactViews>(
			services => new DirectoryCompactViews(
				services.GetRequiredService<IMongoCollection<DirectoryCompactDocument>>(),
				services.GetRequiredOptions<MongoDbFilesViewsOptions>().Json
			)
		);
		@this.AddScoped<IDirectoryCompactViewsConsistency>(
			services => new DirectoryCompactViewsConsistency(
				services.GetRequiredService<IMongoCollection<DirectoryCompactDocument>>(),
				services.GetRequiredOptions<MongoDbFilesViewsOptions>().Json
			)
		);

		@this.AddScoped<IFileCompactViews, FileCompactViews>();
		@this.AddScoped<IFileCompactViewsConsistency, FileCompactViewsConsistency>();

		@this.AddScoped<IFileDetailViews, FileDetailViews>();
		@this.AddScoped<IFileDetailViewsConsistency, FileDetailViewsConsistency>();

		@this.AddScoped<IStorageCompactViews>(
			services => new StorageCompactViews(services.GetRequiredService<IMongoCollection<StorageCompactDocument>>())
		);
		@this.AddScoped<IStorageCompactViewsConsistency>(
			services => new StorageCompactConsistency(
				services.GetRequiredService<IMongoCollection<StorageCompactDocument>>(),
				services.GetRequiredOptions<MongoDbFilesViewsOptions>().Json
			)
		);

		@this.AddScoped<IUserCompactViews, UserCompactViews>();
		@this.AddScoped<IUserCompactViewsConsistency, UserCompactConsistency>();

		@this.AddScoped<IMongoCollection<DirectoryCompactDocument>>(
			services => services
				.GetRequiredService<IMongoDatabase>()
				.GetCollection<DirectoryCompactDocument>("directory-compact")
		);
		@this.AddScoped<IMongoCollection<StorageCompactDocument>>(
			services => services
				.GetRequiredService<IMongoDatabase>()
				.GetCollection<StorageCompactDocument>("storage-compact")
		);
		@this.AddScoped<IMongoDatabase>(
			services => services
				.GetRequiredService<IMongoClient>()
				.GetDatabase("files-views")
		);
		@this.AddScoped<IMongoClient>(
			services =>
			{
				var settings = services.GetRequiredOptions<MongoDbFilesViewsOptions>().Settings;

				return settings is not null ? new MongoClient(settings) : new MongoClient();
			}
		);
	}
}