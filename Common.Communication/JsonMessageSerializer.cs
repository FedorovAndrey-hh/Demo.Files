using System.Text.Json;
using Common.Core.Diagnostics;

namespace Common.Communication;

public sealed class JsonMessageSerializer : IMessageSerializer
{
	public JsonMessageSerializer(JsonSerializerOptions? jsonOptions)
	{
		_jsonOptions = jsonOptions;
	}

	private readonly JsonSerializerOptions? _jsonOptions;

	public byte[] SerializeMessage(object message)
	{
		Preconditions.RequiresNotNull(message, nameof(message));

		return JsonSerializer.SerializeToUtf8Bytes(message, _jsonOptions);
	}

	public T DeserializeMessage<T>(byte[] message)
		where T : notnull
		=> JsonSerializer.Deserialize<T>(message, _jsonOptions)
		   ?? throw new JsonException();
}