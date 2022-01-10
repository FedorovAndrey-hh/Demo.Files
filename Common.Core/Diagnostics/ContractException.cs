using System.Runtime.Serialization;

namespace Common.Core.Diagnostics;

public class ContractException : Exception
{
	protected ContractException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
	}

	public ContractException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
	}
}