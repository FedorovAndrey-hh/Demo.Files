using System.Text.Json;
using MongoDB.Driver;

namespace Demo.Files.Query.Views.MongoDb;

public sealed class MongoDbFilesViewsOptions
{
	public JsonSerializerOptions? Json { get; set; }

	public MongoClientSettings? Settings { get; set; }

	public string? Host { get; set; }
	public int? Port { get; set; }
	public string? Database { get; set; }
	public string? Username { get; set; }
	public string? Password { get; set; }
}