using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Common.EntityFramework.Postgres;

public sealed class NpgsqlLogging
{
	private static object _lock = new();
	private static bool _isPostgresLoggingInitialized;

	public static bool TryEnableLogging(ILoggerFactory loggerFactory, bool allowSensitiveInformationReveal)
	{
		Preconditions.RequiresNotNull(loggerFactory, nameof(loggerFactory));

		lock (_lock)
		{
			if (!_isPostgresLoggingInitialized)
			{
				_isPostgresLoggingInitialized = true;

				try
				{
					NpgsqlLogManager.Provider = loggerFactory.ToNpgsqlLoggerProvider();
					NpgsqlLogManager.IsParameterLoggingEnabled = allowSensitiveInformationReveal;
					return true;
				}
				catch
				{
					return false;
				}
			}

			return false;
		}
	}
}