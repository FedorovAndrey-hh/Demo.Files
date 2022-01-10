using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Abstractions;

public interface IAsUserAcquireStorageResource
{
	public Task<User> ExecuteAsync(
		IUserId userId,
		UserVersion? userVersion,
		Resource.Storage resource);
}