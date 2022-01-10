using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Abstractions;

public interface IAuthorizationContext
{
	public User.IWriteContext ForUser();
}