using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Query.Views.Users;

public sealed class UserCompact
{
	[JsonConstructor]
	public UserCompact(long id, string email, string username, long? storageId, ulong version)
	{
		Preconditions.RequiresNotNull(email, nameof(email));
		Preconditions.RequiresNotNull(username, nameof(username));

		Id = id;
		Email = email;
		Username = username;
		StorageId = storageId;
		Version = version;
	}

	public long Id { get; set; }

	public string Email { get; set; }
	public string Username { get; set; }

	public long? StorageId { get; set; }

	public ulong Version { get; set; }
}