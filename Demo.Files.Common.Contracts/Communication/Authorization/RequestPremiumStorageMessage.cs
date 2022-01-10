using System.Text.Json.Serialization;
using Common.Core.Diagnostics;

namespace Demo.Files.Common.Contracts.Communication.Authorization;

public sealed class RequestPremiumStorageMessage
{
	public const string Port = "auth-out:request-premium-storage";

	[JsonConstructor]
	public RequestPremiumStorageMessage(string id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		Id = id;
	}

	public string Id { get; set; }
}