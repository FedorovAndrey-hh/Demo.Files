using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Demo.Files.Common.Configuration;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;

public static class Serialization
{
	public static void Configure(JsonSerializerOptions options)
	{
		Preconditions.RequiresNotNull(options, nameof(options));

		SerializationConfiguration.ConfigureJson(options);

		options.Converters.Add(new UserIdAsLongConverter());
		options.Converters.Add(new StorageIdAsLongConverter());
		options.Converters.Add(new ResourceRequestIdAsLongConverter());
		options.Converters.Add(new UsernameAsStringConverter());
	}

	public sealed class UserIdAsLongConverter : JsonConverter<UserId>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			Preconditions.RequiresNotNull(typeToConvert, nameof(typeToConvert));

			return Eq.Value(typeof(UserId), typeToConvert);
		}

		public override UserId Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return UserId.Of(reader.GetInt64());
		}

		public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}
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
		
	public sealed class ResourceRequestIdAsLongConverter : JsonConverter<ResourceRequestId>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			Preconditions.RequiresNotNull(typeToConvert, nameof(typeToConvert));

			return Eq.Value(typeof(ResourceRequestId), typeToConvert);
		}

		public override ResourceRequestId Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return ResourceRequestId.Of(reader.GetInt64());
		}

		public override void Write(Utf8JsonWriter writer, ResourceRequestId value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}
	}

	public sealed class UsernameAsStringConverter : JsonConverter<Username>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			Preconditions.RequiresNotNull(typeToConvert, nameof(typeToConvert));

			return Eq.Value(typeof(Username), typeToConvert);
		}

		public override Username Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			return Username.Parser.Parse(reader.GetString(), out var result, out _)
				? result
				: throw new UserException(UserError.UsernameFormat);
		}

		public override void Write(Utf8JsonWriter writer, Username value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString(Username.Formatter));
		}
	}
}