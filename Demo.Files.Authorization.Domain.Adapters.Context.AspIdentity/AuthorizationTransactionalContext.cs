using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

internal class AuthorizationTransactionalContext : IAuthorizationTransactionalContext
{
	internal AuthorizationTransactionalContext(
		AuthorizationDbContext dbContext,
		UserManager<UserData> userManager,
		IDbContextTransaction dbContextTransaction,
		IExceptionWrapper exceptionWrapper)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(userManager, nameof(userManager));
		Preconditions.RequiresNotNull(dbContextTransaction, nameof(dbContextTransaction));
		Preconditions.RequiresNotNull(exceptionWrapper, nameof(exceptionWrapper));

		_dbContext = dbContext;
		_userManager = userManager;
		_dbContextTransaction = dbContextTransaction;
		_exceptionWrapper = exceptionWrapper;
	}

	private readonly AuthorizationDbContext _dbContext;
	private readonly UserManager<UserData> _userManager;
	private readonly IDbContextTransaction _dbContextTransaction;
	private readonly IExceptionWrapper _exceptionWrapper;

	public async Task CommitAsync()
	{
		try
		{
			await _dbContextTransaction.CommitAsync().ConfigureAwait(false);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	public Task RollbackAsync() => _dbContextTransaction.RollbackAsync();

	public ValueTask DisposeAsync() => _dbContextTransaction.DisposeAsync();

	public User.IWriteContext ForUser() => new UserWriteContext(_dbContext, _userManager, _exceptionWrapper);
}