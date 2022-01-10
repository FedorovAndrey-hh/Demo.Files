using System.Runtime.Serialization;
using Common.Core.Diagnostics;

namespace Common.Core;

public class EnvironmentException : Exception
{
	public static EnvironmentException VariableNotFound(string name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		return new EnvironmentException($"Environment variable `{name}` not found.");
	}

	protected EnvironmentException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
	}

	public EnvironmentException(string? message = null, Exception? innerException = null)
		: base(message, innerException)
	{
	}
}