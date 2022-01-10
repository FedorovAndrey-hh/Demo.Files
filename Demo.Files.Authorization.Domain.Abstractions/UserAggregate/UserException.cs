using System.Runtime.Serialization;
using Common.Core;
using Common.Core.Diagnostics;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed class UserException : Exception
{
	public UserException(SerializationInfo info, StreamingContext context)
		: base(Preconditions.RequiresNotNull(info, nameof(info)), context)
	{
		Error = (UserError)info.GetInt32("UserError");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		Preconditions.RequiresNotNull(info, nameof(info));
		base.GetObjectData(info, context);

		info.AddValue("UserError", (Int32)Error);
	}

	public UserException(UserError error, string? message = null, Exception? innerException = null)
		: base(Strings.JoinSkipNulls(" ", error.ToString(), message), innerException)
	{
		Error = error;
	}

	public UserError Error { get; }
}