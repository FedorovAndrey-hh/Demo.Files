using System.ComponentModel.DataAnnotations;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public class ResourceRequestData
{
	public long Id { get; set; }

	[Required]
	public string Type { get; set; } = null!;

	public long OwnerId { get; set; }
}