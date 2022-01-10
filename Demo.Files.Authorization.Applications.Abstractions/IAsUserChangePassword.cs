using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Abstractions;

public interface IAsUserChangePassword
{
	public Task<User> ExecuteAsync(
		IUserId userId,
		UserVersion? userVersion,
		string currentPassword,
		string newPassword);
}