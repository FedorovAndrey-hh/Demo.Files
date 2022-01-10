using Common.Core.Diagnostics;
using Common.Persistence;
using Demo.Files.Authorization.Domain.Abstractions;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

public sealed class AuthorizationWorkScopeProvider : TransactionalWorkScopeProvider<IAuthorizationContext>
{
	public AuthorizationWorkScopeProvider(IAuthorizationPersistenceContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		_context = context;
	}

	private readonly IAuthorizationPersistenceContext _context;

	protected override async Task<ITransaction> BeginTransactionAsync()
		=> await _context.BeginTransactionAsync().ConfigureAwait(false);

	protected override IAuthorizationContext ScopeOf(ITransaction transaction)
	{
		Preconditions.RequiresNotNull(transaction, nameof(transaction));

		return (IAuthorizationTransactionalContext)transaction;
	}
}