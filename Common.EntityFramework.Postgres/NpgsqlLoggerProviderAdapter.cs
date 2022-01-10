using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace Common.EntityFramework.Postgres;

public sealed class NpgsqlLoggerProviderAdapter : INpgsqlLoggingProvider
{
	public NpgsqlLoggerProviderAdapter(ILoggerFactory implementation)
	{
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		_implementation = implementation;
	}

	private readonly ILoggerFactory _implementation;

	public NpgsqlLogger CreateLogger(string name) => _implementation.CreateLogger(name).ToNpgsqlLogger();
}