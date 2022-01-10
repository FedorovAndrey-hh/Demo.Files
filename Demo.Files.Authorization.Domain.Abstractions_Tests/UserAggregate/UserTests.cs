using Common.Core.Modifications;
using FluentAssertions;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public abstract class UserTests
{
	protected abstract User.IWriteContext Context { get; }

	private async Task<User> _CreateUserAsync(UserEmail? userEmail = null, UserDisplayName? displayName = null)
		=> User.After(
			await User.RegisterAsync(
				Context,
				userEmail ?? UserEmail.Create("test@yandex.ru"),
				displayName ?? UserDisplayName.Create("test"),
				Password.Create("000000")
			)
		);

	public virtual async Task Register_WithValidParameters_CreatesUser()
	{
		var user = await _CreateUserAsync();

		user.Should().NotBeNull();
	}

	protected abstract Task<Resource> HandleResourceRequestAsync(UserEvent.Modified.ResourceRequested @event);

	public virtual async Task RequestResource_WithUniqueType_CanAcquireResource()
	{
		var user = await _CreateUserAsync();

		var resourceRequested = await user.RequestResourceAsync(Context, ResourceType.Storage);
		user = user.After(resourceRequested);
		var resource = await HandleResourceRequestAsync(resourceRequested);
		user = await user.AfterActionAsync(e => e.AcquireResourceAsync(Context, resource));

		user.OwnResource(resource).Should().BeTrue();
	}

	public virtual async Task Register_WithExistingEmail_ThrowsEmailConflictError()
	{
		var email = UserEmail.Create("test@yandex.ru");
		await _CreateUserAsync(userEmail: email);

		UserError? actual = null;
		try
		{
			await _CreateUserAsync(userEmail: email);
		}
		catch (UserException e)
		{
			actual = e.Error;
		}

		actual.Should().Be(UserError.EmailConflict);
	}

	public virtual async Task Register_WithExistingDisplayName_CreatesUsersWithUniqueUsernames()
	{
		var displayName = UserDisplayName.Create("test");
		var user1 = await _CreateUserAsync(
			displayName: displayName,
			userEmail: UserEmail.Create("test1@yandex.ru")
		);
		var user2 = await _CreateUserAsync(
			displayName: displayName,
			userEmail: UserEmail.Create("test2@yandex.ru")
		);

		user1.Username.Should().NotBe(user2.Username);
		user1.Username.DisplayName.Should().Be(displayName);
		user2.Username.DisplayName.Should().Be(displayName);
	}

	public virtual async Task ChangeUsername_Concurrently_ThrowsOutdatedError()
	{
		var registered = await User.RegisterAsync(
			Context,
			UserEmail.Create("test@yandex.ru"),
			UserDisplayName.Create("test"),
			Password.Create("000000")
		);

		var user1 = User.After(registered);
		var user2 = User.After(registered);

		await user1.ChangeUsernameAsync(Context, UserDisplayName.Create("test2"));

		UserError? error = null;
		try
		{
			await user2.ChangeUsernameAsync(Context, UserDisplayName.Create("test2"));
		}
		catch (UserException e)
		{
			error = e.Error;
		}

		error.Should().Be(UserError.Outdated);
	}
}