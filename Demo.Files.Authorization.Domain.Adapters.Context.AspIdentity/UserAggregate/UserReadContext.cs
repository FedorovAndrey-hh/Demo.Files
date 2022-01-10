using System.Collections.Immutable;
using Common.Core.Diagnostics;
using Common.Core.Emails;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

internal sealed class UserReadContext : User.IReadContext
{
	public UserReadContext(AuthorizationDbContext dbContext, UserManager<UserData> userManager)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(userManager, nameof(userManager));

		_dbContext = dbContext;
		_userManager = userManager;
	}

	private readonly AuthorizationDbContext _dbContext;
	private readonly UserManager<UserData> _userManager;

	async Task<User?> User.IReadContext.FindAsync(IUserId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		var rawId = id.RawLong();
		var userData = await _dbContext.Users
			.Include(e => e.ResourceRequests)
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == rawId)
			.ConfigureAwait(false);

		if (userData is null)
		{
			return null;
		}

		return _FromSnapshot(userData);
	}

	async Task<User?> User.IReadContext.FindAsync(Username username)
	{
		Preconditions.RequiresNotNull(username, nameof(username));

		var normalizedName = _userManager.NormalizeName(username.ToString(Username.Formatter));
		var userData = await _dbContext.Users
			.Include(e => e.ResourceRequests)
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.NormalizedUserName == normalizedName)
			.ConfigureAwait(false);

		if (userData is null)
		{
			return null;
		}

		return _FromSnapshot(userData);
	}

	async Task<User?> User.IReadContext.FindAsync(UserEmail userEmail)
	{
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));

		var normalizedEmail = _userManager.NormalizeEmail(userEmail.Value.ToString(EmailFormatter.Create()));
		var userData = await _dbContext.Users
			.Include(e => e.ResourceRequests)
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.NormalizedEmail == normalizedEmail)
			.ConfigureAwait(false);

		if (userData is null)
		{
			return null;
		}

		return _FromSnapshot(userData);
	}

	private User _FromSnapshot(UserData snapshot)
	{
		Preconditions.RequiresNotNull(snapshot, nameof(snapshot));

		return User.IReadContext.FromSnapshot(
			snapshot.GetId(),
			snapshot.GetVersion(),
			snapshot.GetEmail(),
			snapshot.GetUsername(),
			snapshot.IsActive,
			snapshot.ResourceRequests
				.Select(e => ResourceRequest.Create(e.GetId(), e.GetResourceType()))
				.ToImmutableList(),
			snapshot.GetResources()
		);
	}
}