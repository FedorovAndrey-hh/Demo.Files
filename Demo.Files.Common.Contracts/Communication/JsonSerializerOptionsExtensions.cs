using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Common.Contracts.Communication;

public static class JsonSerializerOptionsExtensions
{
	public static void AddFilesCommunicationAdapters(this JsonSerializerOptions @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		
		@this.Converters.Add(new StorageIdAdapter());
		@this.Converters.Add(new StorageVersionAdapter());
		@this.Converters.Add(new DirectoryIdAdapter());
	}

	private sealed class StorageIdAdapter : JsonConverter<StorageId>
	{
		public override StorageId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			=> StorageId.Of(reader.GetInt64());

		public override void Write(Utf8JsonWriter writer, StorageId value, JsonSerializerOptions options)
			=> writer.WriteNumberValue(value.Value);
	}
	
	private sealed class DirectoryIdAdapter : JsonConverter<DirectoryId>
	{
		public override DirectoryId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			=> DirectoryId.Of(reader.GetInt64());

		public override void Write(Utf8JsonWriter writer, DirectoryId value, JsonSerializerOptions options)
			=> writer.WriteNumberValue(value.Value);
	}
	
	private sealed class StorageVersionAdapter : JsonConverter<StorageVersion>
	{
		public override StorageVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			=> StorageVersion.Of(reader.GetUInt64());

		public override void Write(Utf8JsonWriter writer, StorageVersion value, JsonSerializerOptions options)
			=> writer.WriteNumberValue(value.Value);
	}
}