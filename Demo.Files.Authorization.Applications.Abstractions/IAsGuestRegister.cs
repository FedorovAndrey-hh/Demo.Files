using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Abstractions;

public interface IAsGuestRegister
{
	public Task<User> ExecuteAsync(string email, string displayName, string password);
}