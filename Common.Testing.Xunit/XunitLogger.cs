using System.Text;
using Common.Core;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Common.Testing.Xunit;

public sealed class XunitLogger : ILogger
{
	private static readonly string[] _newLineChars = { Environment.NewLine };
	private readonly string _category;
	private readonly LogLevel _minLogLevel;
	private readonly ITestOutputHelper _output;

	public XunitLogger(ITestOutputHelper output, string category, LogLevel minLogLevel)
	{
		_minLogLevel = minLogLevel;
		_category = category;
		_output = output;
	}

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		// Buffer the message into a single string in order to avoid shearing the message when running across multiple threads.
		var messageBuilder = new StringBuilder();

		var timestamp = DateTime.Now.ToString("s");
		var firstLinePrefix = $"| [{timestamp}] {_category} {logLevel}: ";
		var lines = formatter(state, exception).Split(_newLineChars, StringSplitOptions.RemoveEmptyEntries);
		messageBuilder.AppendLine(firstLinePrefix + lines.FirstOrDefault().OrEmpty());

		var additionalLinePrefix = "|" + new string(' ', firstLinePrefix.Length - 1);
		foreach (var line in lines.Skip(1))
		{
			messageBuilder.AppendLine(additionalLinePrefix + line);
		}

		if (exception != null)
		{
			lines = exception.ToString().Split(_newLineChars, StringSplitOptions.RemoveEmptyEntries);
			additionalLinePrefix = "| ";
			foreach (var line in lines.Skip(1))
			{
				messageBuilder.AppendLine(additionalLinePrefix + line);
			}
		}

		// Remove the last line-break, because ITestOutputHelper only has WriteLine.
		var message = messageBuilder.ToString();
		if (message.EndsWith(Environment.NewLine))
		{
			message = message.Substring(0, message.Length - Environment.NewLine.Length);
		}

		try
		{
			_output.WriteLine(message);
		}
		catch (Exception)
		{
			// We could fail because we're on a background thread and our captured ITestOutputHelper is
			// busted (if the test "completed" before the background thread fired).
			// So, ignore this. There isn't really anything we can do but hope the
			// caller has additional loggers registered
		}
	}

	public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

	public IDisposable BeginScope<TState>(TState state) => new NullScope();

	private sealed class NullScope : IDisposable
	{
		public void Dispose()
		{
		}
	}
}