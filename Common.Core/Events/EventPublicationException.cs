using System.Runtime.Serialization;

namespace Common.Core.Events;

public class EventPublicationException : AggregateException
{
	public EventPublicationException()
	{
	}

	public EventPublicationException(IEnumerable<Exception> innerExceptions)
		: base(innerExceptions)
	{
	}

	public EventPublicationException(params Exception[] innerExceptions)
		: base(innerExceptions)
	{
	}

	protected EventPublicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public EventPublicationException(string? message)
		: base(message)
	{
	}

	public EventPublicationException(string? message, IEnumerable<Exception> innerExceptions)
		: base(message, innerExceptions)
	{
	}

	public EventPublicationException(string? message, Exception innerException)
		: base(message, innerException)
	{
	}

	public EventPublicationException(string? message, params Exception[] innerExceptions)
		: base(message, innerExceptions)
	{
	}
}