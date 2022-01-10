using System.Runtime.Serialization;
using Common.Core;
using Common.Core.Diagnostics;

namespace Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;

public class ContainerException : Exception
{
	protected ContainerException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
		Error = (ContainerError)info.GetInt32("ContainerError");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		Preconditions.RequiresNotNull(info, nameof(info));
		base.GetObjectData(info, context);

		info.AddValue("ContainerError", (Int32)Error);
	}

	public ContainerException(ContainerError error, string? message = null, Exception? innerException = null)
		: base(Strings.JoinSkipNulls(" ", error.ToString(), message), innerException)
	{
		Error = error;
	}

	public ContainerError Error { get; }
}