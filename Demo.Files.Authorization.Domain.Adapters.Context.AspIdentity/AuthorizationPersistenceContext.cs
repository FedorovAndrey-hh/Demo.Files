using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Microsoft.AspNetCore.Identity;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

public sealed class AuthorizationPersistenceContext : IAuthorizationPersistenceContext
{
	public AuthorizationPersistenceContext(AuthorizationDbContext dbContext, UserManager<UserData> userManager)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(userManager, nameof(userManager));

		_dbContext = dbContext;
		_userManager = userManager;
	}

	private readonly AuthorizationDbContext _dbContext;

	private readonly UserManager<UserData> _userManager;

	public async Task<IAuthorizationTransactionalContext> BeginTransactionAsync()
		=> new AuthorizationTransactionalContext(
			_dbContext,
			_userManager,
			await _dbContext.Database.BeginTransactionAsync().ConfigureAwait(false),
			AuthorizationDbExceptionWrapper.Create()
		);
}