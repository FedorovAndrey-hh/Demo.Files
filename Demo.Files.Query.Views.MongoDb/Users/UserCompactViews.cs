using Demo.Files.Query.Views.Users;

namespace Demo.Files.Query.Views.MongoDb.Users;

public sealed class UserCompactViews
	: IUserCompactViews
{
	public Task<string> FindByIdAsync(long id)
	{
		throw new NotImplementedException();
	}
}