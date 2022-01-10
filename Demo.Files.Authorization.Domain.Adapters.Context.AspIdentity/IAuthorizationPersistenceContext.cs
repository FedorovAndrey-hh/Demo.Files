namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

public interface IAuthorizationPersistenceContext
{
	public Task<IAuthorizationTransactionalContext> BeginTransactionAsync();
}