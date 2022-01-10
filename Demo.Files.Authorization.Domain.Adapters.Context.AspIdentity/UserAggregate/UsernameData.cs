using System.ComponentModel.DataAnnotations;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public class UsernameData
{
	[Key]
	public string DisplayName { get; set; } = null!;

	public ushort LastQuantifier { get; set; }
}