using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Common.EntityFramework.Postgres;

public static class LoggerFactoryExtensions
{
	public static INpgsqlLoggingProvider ToNpgsqlLoggerProvider(this ILoggerFactory @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return new NpgsqlLoggerProviderAdapter(@this);
	}
}