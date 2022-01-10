using System.Text.Json;
using Common.Core.Diagnostics;
using Demo.Files.Common.Configuration;

namespace Demo.Files.Query.WebApi.Utility.Behavior;

public static class Serialization
{
	public static void Configure(JsonSerializerOptions options)
	{
		Preconditions.RequiresNotNull(options, nameof(options));

		SerializationConfiguration.ConfigureJson(options);
	}
}