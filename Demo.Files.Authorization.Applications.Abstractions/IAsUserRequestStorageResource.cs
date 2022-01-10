using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Abstractions;

public interface IAsUserRequestStorageResource
{
	public Task<(User, IResourceRequestId)> ExecuteAsync(
		IUserId userId,
		UserVersion? userVersion);
}