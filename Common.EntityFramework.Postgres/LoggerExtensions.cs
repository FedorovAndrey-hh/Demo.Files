using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Common.EntityFramework.Postgres;

public static class LoggerExtensions
{
	public static NpgsqlLogger ToNpgsqlLogger(this ILogger @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return new NpgsqlLoggerAdapter(@this);
	}
}