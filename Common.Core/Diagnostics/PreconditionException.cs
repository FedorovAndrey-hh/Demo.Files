using System.Runtime.Serialization;

namespace Common.Core.Diagnostics;

public sealed class PreconditionException : ContractException
{
	public PreconditionException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
	}

	public PreconditionException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
	}
}