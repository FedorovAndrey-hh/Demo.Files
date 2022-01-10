using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Common.Testing.Xunit;

public static class XunitLoggerFactoryExtensions
{
	public static ILoggingBuilder AddXunit(this ILoggingBuilder builder, ITestOutputHelper output)
	{
		Preconditions.RequiresNotNull(output, nameof(output));
		Preconditions.RequiresNotNull(builder, nameof(builder));

		builder.AddProvider(new XunitLoggerProvider(output));
		return builder;
	}

	public static ILoggingBuilder AddXunit(
		this ILoggingBuilder builder,
		ITestOutputHelper output,
		LogLevel minLevel)
	{
		Preconditions.RequiresNotNull(builder, nameof(builder));
		Preconditions.RequiresNotNull(output, nameof(output));

		builder.AddProvider(new XunitLoggerProvider(output, minLevel));
		return builder;
	}

	public static ILoggerFactory AddXunit(this ILoggerFactory loggerFactory, ITestOutputHelper output)
	{
		Preconditions.RequiresNotNull(loggerFactory, nameof(loggerFactory));
		Preconditions.RequiresNotNull(output, nameof(output));

		loggerFactory.AddProvider(new XunitLoggerProvider(output));
		return loggerFactory;
	}

	public static ILoggerFactory AddXunit(
		this ILoggerFactory loggerFactory,
		ITestOutputHelper output,
		LogLevel minLevel)
	{
		Preconditions.RequiresNotNull(loggerFactory, nameof(loggerFactory));
		Preconditions.RequiresNotNull(output, nameof(output));

		loggerFactory.AddProvider(new XunitLoggerProvider(output, minLevel));
		return loggerFactory;
	}
}