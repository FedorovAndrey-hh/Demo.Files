using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

public sealed class InternalUserStore : UserStore<UserData, UserRoleData, AuthorizationDbContext, long>
{
	public InternalUserStore(AuthorizationDbContext context, IdentityErrorDescriber describer)
		: base(context, describer)
	{
	}

	public override async Task<IdentityResult> UpdateAsync(
		UserData user,
		CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		ThrowIfDisposed();
		if (user == null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		var isAttachedToContext = Context.Users.Local.Contains(user);

		if (!isAttachedToContext)
		{
			Context.Attach(user);
		}

		user.ConcurrencyStamp = Guid.NewGuid().ToString();
		if (!isAttachedToContext)
		{
			Context.Update(user);
		}

		try
		{
			await SaveChanges(cancellationToken);
		}
		catch (DbUpdateConcurrencyException)
		{
			return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
		}

		return IdentityResult.Success;
	}
}