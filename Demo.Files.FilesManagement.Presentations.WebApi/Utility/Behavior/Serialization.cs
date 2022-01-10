using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Common.Configuration;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;

namespace Demo.Files.FilesManagement.Presentations.WebApi.Utility.Behavior;

public static class Serialization
{
	public static void Configure(JsonSerializerOptions options)
	{
		Preconditions.RequiresNotNull(options, nameof(options));

		SerializationConfiguration.ConfigureJson(options);

		options.Converters.Add(new StorageIdAsLongConverter());
		options.Converters.Add(new DirectoryIdAsLongConverter());
		options.Converters.Add(new FileIdAsLongConverter());
	}

	public sealed class StorageIdAsLongConverter : JsonConverter<StorageId>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			Preconditions.RequiresNotNull(typeToConvert, nameof(typeToConvert));

			return Eq.Value(typeof(StorageId), typeToConvert);
		}

		public override StorageId Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return StorageId.Of(reader.GetInt64());
		}

		public override void Write(Utf8JsonWriter writer, StorageId value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}
	}

	public sealed class DirectoryIdAsLongConverter : JsonConverter<DirectoryId>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			Preconditions.RequiresNotNull(typeToConvert, nameof(typeToConvert));

			return Eq.Value(typeof(DirectoryId), typeToConvert);
		}

		public override DirectoryId Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return DirectoryId.Of(reader.GetInt64());
		}

		public override void Write(Utf8JsonWriter writer, DirectoryId value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}
	}

	public sealed class FileIdAsLongConverter : JsonConverter<FileId>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			Preconditions.RequiresNotNull(typeToConvert, nameof(typeToConvert));

			return Eq.Value(typeof(DirectoryId), typeToConvert);
		}

		public override FileId Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return FileId.Of(reader.GetInt64());
		}

		public override void Write(Utf8JsonWriter writer, FileId value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}
	}
}