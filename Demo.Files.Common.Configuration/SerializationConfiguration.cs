using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Common.Core.Diagnostics;

namespace Demo.Files.Common.Configuration;

public static class SerializationConfiguration
{
	public static JsonSerializerOptions ConfigureJson()
	{
		var result = new JsonSerializerOptions();
		ConfigureJson(result);
		return result;
	}

	public static void ConfigureJson(JsonSerializerOptions options)
	{
		Preconditions.RequiresNotNull(options, nameof(options));

		options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
		options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		options.NumberHandling = JsonNumberHandling.Strict;
		options.AllowTrailingCommas = false;
		options.ReadCommentHandling = JsonCommentHandling.Skip;
		
		options.Converters.Add(new JsonStringEnumConverter());
	}
}