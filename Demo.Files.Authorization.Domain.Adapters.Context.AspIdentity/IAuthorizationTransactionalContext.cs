using Common.Persistence;
using Demo.Files.Authorization.Domain.Abstractions;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity;

public interface IAuthorizationTransactionalContext
	: IAuthorizationContext,
	  ITransaction
{
}