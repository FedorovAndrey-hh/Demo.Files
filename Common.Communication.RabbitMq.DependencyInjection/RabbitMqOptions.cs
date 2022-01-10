using System.Text.Json;

namespace Common.Communication.RabbitMq.DependencyInjection;

public sealed class RabbitMqOptions
{
	public string? Host { get; set; }
	public int? Port { get; set; }

	public string? Username { get; set; }
	public string? Password { get; set; }

	public bool? Persistent { get; set; }
	public bool? ResendOnFailedHandle { get; set; }
	public TimeSpan ReadyCheckPeriod { get; set; }

	public JsonSerializerOptions? Json { get; set; }
}