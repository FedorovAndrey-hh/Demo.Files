using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Common.Core.Text;

public sealed class JsonRawEnumerable
{
	public JsonRawEnumerable(IEnumerable<string> rawItems)
	{
		Preconditions.RequiresNotNull(rawItems, nameof(rawItems));

		_rawItems = rawItems;
	}

	private readonly IEnumerable<string> _rawItems;

	public sealed class Converter : JsonConverter<JsonRawEnumerable>
	{
		public override JsonRawEnumerable Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			throw Errors.UnsupportedOperation(nameof(Read));
		}

		public override void Write(Utf8JsonWriter writer, JsonRawEnumerable value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();

			foreach (var rawItem in value._rawItems)
			{
				writer.WriteRawValue(rawItem);
			}

			writer.WriteEndArray();
		}
	}
}