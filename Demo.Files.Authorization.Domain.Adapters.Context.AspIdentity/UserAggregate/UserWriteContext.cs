using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Execution.Decoration;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

internal sealed class UserWriteContext : User.IWriteContext
{
	public UserWriteContext(
		AuthorizationDbContext dbContext,
		UserManager<UserData> userManager,
		IExceptionWrapper exceptionWrapper)
	{
		Preconditions.RequiresNotNull(dbContext, nameof(dbContext));
		Preconditions.RequiresNotNull(userManager, nameof(exceptionWrapper));
		Preconditions.RequiresNotNull(exceptionWrapper, nameof(userManager));

		_dbContext = dbContext;
		_userManager = userManager;
		_exceptionWrapper = exceptionWrapper;
	}

	private readonly AuthorizationDbContext _dbContext;
	private readonly UserManager<UserData> _userManager;
	private readonly IExceptionWrapper _exceptionWrapper;

	async Task<UserEvent.Registered> User.IWriteContext.RegisterAsync(
		UserEmail userEmail,
		UserDisplayName displayName,
		Password password)
	{
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));
		Preconditions.RequiresNotNull(displayName, nameof(displayName));
		Preconditions.RequiresNotNull(password, nameof(password));

		try
		{
			var quantifier = await _GetNextUsernameQuantifierAsync(displayName).ConfigureAwait(false);
			var username = Username.Create(displayName, quantifier);

			var userData = new UserData();
			userData.SetEmail(userEmail);
			userData.SetUsername(username);
			userData.IsActive = true;
			userData.SetVersion(UserVersion.Initial);
			(await _userManager
					.CreateAsync(userData, password.AsString())
					.ConfigureAwait(false))
				.ThrowIfFailed();

			return new UserEvent.Registered(
				userData.GetId(),
				userData.GetVersion(),
				userEmail,
				username
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.ResourceRequested> User.IWriteContext.RequestResourceAsync(
		User user,
		ResourceType type)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		try
		{
			var request = new ResourceRequestData();
			request.SetResourceType(type);
			request.SetOwnerId(user.Id.Concrete());
			_dbContext.ResourceRequests.Add(request);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);
			(await _userManager.UpdateAsync(userData).ConfigureAwait(false)).ThrowIfFailed();

			return new UserEvent.Modified.ResourceRequested(
				user.Id,
				userData.GetVersion(),
				ResourceRequest.Create(request.GetId(), type)
			);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.ResourceAcquired> User.IWriteContext.AcquireResourceAsync(
		User user,
		Resource resource)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		try
		{
			var request = await _dbContext.ResourceRequests.FirstOrDefaultAsync().ConfigureAwait(false)
			              ?? throw new UserException(UserError.ResourceNotRequested);
			_dbContext.ResourceRequests.Remove(request);
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);

			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);
			userData.AddResource(resource);
			(await _userManager.UpdateAsync(userData).ConfigureAwait(false)).ThrowIfFailed();

			return new UserEvent.Modified.ResourceAcquired(user.Id, userData.GetVersion(), resource);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.ResourceDeleted> User.IWriteContext.DeleteResourceAsync(User user, Resource resource)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		try
		{
			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);
			userData.RemoveResource(resource);
			(await _userManager.UpdateAsync(userData).ConfigureAwait(false)).ThrowIfFailed();

			return new UserEvent.Modified.ResourceDeleted(user.Id, userData.GetVersion(), resource);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.PasswordChanged> User.IWriteContext.ChangePasswordAsync(
		User user,
		Password currentPassword,
		Password newPassword)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(currentPassword, nameof(currentPassword));
		Preconditions.RequiresNotNull(newPassword, nameof(newPassword));

		try
		{
			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);

			(await _userManager
					.ChangePasswordAsync(
						userData,
						currentPassword.AsString(),
						newPassword.AsString()
					)
					.ConfigureAwait(false))
				.ThrowIfFailed();

			return new UserEvent.Modified.PasswordChanged(user.Id, userData.GetVersion());
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.UsernameChanged> User.IWriteContext.ChangeUsernameAsync(
		User user,
		UserDisplayName displayName)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		try
		{
			var quantifier = await _GetNextUsernameQuantifierAsync(displayName).ConfigureAwait(false);
			var username = Username.Create(displayName, quantifier);

			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);
			userData.SetUsername(username);
			(await _userManager.UpdateAsync(userData).ConfigureAwait(false)).ThrowIfFailed();

			return new UserEvent.Modified.UsernameChanged(user.Id, userData.GetVersion(), username);
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.Activated> User.IWriteContext.ActivateAsync(User user)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		try
		{
			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);
			userData.IsActive = true;
			(await _userManager.UpdateAsync(userData).ConfigureAwait(false)).ThrowIfFailed();

			return new UserEvent.Modified.Activated(user.Id, userData.GetVersion());
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	async Task<UserEvent.Modified.Deactivated> User.IWriteContext.DeactivateAsync(User user)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		try
		{
			var userData = await _GetStorageDataAndIncrementVersionAsync(user).ConfigureAwait(false);
			userData.IsActive = false;
			(await _userManager.UpdateAsync(userData).ConfigureAwait(false)).ThrowIfFailed();

			return new UserEvent.Modified.Deactivated(user.Id, userData.GetVersion());
		}
		catch (Exception e) when (_exceptionWrapper.ShouldBeWrapped(e))
		{
			throw _exceptionWrapper.Wrap(e);
		}
	}

	private async Task<UserData> _GetStorageDataAndIncrementVersionAsync(User user)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		var userData =
			await _userManager.FindByIdAsync(user.Id.RawLong().ToString()).ConfigureAwait(false)
			?? throw new UserException(UserError.NotExists);

		var newVersion = user.Version.Increment();
		userData.SetVersion(newVersion);
		UserDataUtility.SetOriginalVersion(_dbContext.Entry(userData), user.Version);

		return userData;
	}

	private async Task<ushort> _GetNextUsernameQuantifierAsync(UserDisplayName displayName)
	{
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		var rawDisplayName = displayName.AsString();

		var usernameData = await _dbContext.Usernames
			.FirstOrDefaultAsync(e => e.DisplayName == rawDisplayName)
			.ConfigureAwait(false);
		if (usernameData is null)
		{
			usernameData = new UsernameData();
			usernameData.DisplayName = rawDisplayName;
			usernameData.LastQuantifier = 0;

			_dbContext.Usernames.Add(usernameData);
		}
		else
		{
			if (Eq.StructSafe(usernameData.LastQuantifier, ushort.MaxValue))
			{
				throw new UserException(UserError.UsernameConflict);
			}

			usernameData.LastQuantifier = (ushort)(usernameData.LastQuantifier + 1);
		}

		await _dbContext.SaveChangesAsync().ConfigureAwait(false);

		return usernameData.LastQuantifier;
	}
}