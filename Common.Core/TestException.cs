using System.Runtime.Serialization;
using Common.Core.Diagnostics;

namespace Common.Core;

public class TestException : Exception
{
	protected TestException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
	}

	public TestException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
	}
}