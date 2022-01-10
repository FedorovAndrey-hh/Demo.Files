namespace Demo.Files.Query.Views.Users;

public interface IUserCompactViews
{
	public Task<string> FindByIdAsync(long id);
}