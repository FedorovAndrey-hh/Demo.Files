using System.Runtime.Serialization;

namespace Common.Core.Diagnostics;

public sealed class PostconditionException : ContractException
{
	public PostconditionException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
	}

	public PostconditionException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
	}
}