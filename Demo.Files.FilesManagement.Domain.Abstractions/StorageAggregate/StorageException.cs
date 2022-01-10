using System.Runtime.Serialization;
using Common.Core;
using Common.Core.Diagnostics;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed class StorageException : Exception
{
	public StorageException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
		Error = (StorageError)info.GetInt32("StorageError");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		Preconditions.RequiresNotNull(info, nameof(info));
		base.GetObjectData(info, context);

		info.AddValue("StorageError", (Int32)Error);
	}

	public StorageException(StorageError error, string? message = null, Exception? innerException = null)
		: base(Strings.JoinSkipNulls(" ", error.ToString(), message), innerException)
	{
		Error = error;
	}

	public StorageError Error { get; }
}