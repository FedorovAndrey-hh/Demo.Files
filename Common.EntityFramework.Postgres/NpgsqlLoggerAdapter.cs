using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Common.EntityFramework.Postgres;

public sealed class NpgsqlLoggerAdapter : NpgsqlLogger
{
	private readonly ILogger _implementation;

	public NpgsqlLoggerAdapter(ILogger implementation)
	{
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		_implementation = implementation;
	}

	public override bool IsEnabled(NpgsqlLogLevel level) => _implementation.IsEnabled(level.ToLogLevel());

	public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception? exception = null)
		=> _implementation.Log(
			level.ToLogLevel(),
			exception,
			"ConnectorId({ConnectorId}) {msg)}",
			connectorId,
			msg
		);
}