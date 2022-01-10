using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Authentication;

public interface IAccessTokenFactory
{
	public Task<string> CreateTokenAsync(User user);
}