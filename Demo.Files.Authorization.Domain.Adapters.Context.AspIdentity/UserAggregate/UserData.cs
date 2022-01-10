using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

[Table("Users")]
[Index(nameof(Version))]
public class UserData : IdentityUser<long>
{
	public bool IsActive { get; set; }

	[ConcurrencyCheck]
	[Column("version")]
	public ulong Version { get; set; }

	public string? Resources { get; set; }

	public List<ResourceRequestData> ResourceRequests { get; set; } = null!;
}