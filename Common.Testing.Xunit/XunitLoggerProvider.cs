using Common.Core.Diagnostics;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Common.Testing.Xunit;

public sealed class XunitLoggerProvider : ILoggerProvider
{
	private readonly ITestOutputHelper _output;
	private readonly LogLevel _minLevel;

	public XunitLoggerProvider(ITestOutputHelper output, LogLevel minLevel = LogLevel.Trace)
	{
		Preconditions.RequiresNotNull(output, nameof(output));

		_output = output;
		_minLevel = minLevel;
	}

	public ILogger CreateLogger(string categoryName) => new XunitLogger(_output, categoryName, _minLevel);

	public void Dispose()
	{
	}
}