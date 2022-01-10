using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

public abstract class AuthorizationDbContext : IdentityDbContext<UserData, UserRoleData, long>
{
	public AuthorizationDbContext(DbContextOptions options)
		: base(Preconditions.RequiresNotNull(options, nameof(options)))
	{
	}

	public AuthorizationDbContext()
	{
	}

	public DbSet<UsernameData> Usernames { get; set; } = null!;
	public DbSet<ResourceRequestData> ResourceRequests { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		Preconditions.RequiresNotNull(builder, nameof(builder));
		base.OnModelCreating(builder);

		builder.Entity<UserData>()
			.HasMany<ResourceRequestData>(e => e.ResourceRequests)
			.WithOne()
			.HasForeignKey(e => e.OwnerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}