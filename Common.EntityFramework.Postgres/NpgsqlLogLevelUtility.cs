using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Common.EntityFramework.Postgres;

public static class NpgsqlLogLevelUtility
{
	public static LogLevel ToLogLevel(this NpgsqlLogLevel @this)
		=> @this switch
		{
			NpgsqlLogLevel.Trace => LogLevel.Trace,
			NpgsqlLogLevel.Debug => LogLevel.Debug,
			NpgsqlLogLevel.Info => LogLevel.Information,
			NpgsqlLogLevel.Warn => LogLevel.Warning,
			NpgsqlLogLevel.Error => LogLevel.Error,
			NpgsqlLogLevel.Fatal => LogLevel.Critical,
			_ => throw Contracts.UnreachableThrow()
		};
}