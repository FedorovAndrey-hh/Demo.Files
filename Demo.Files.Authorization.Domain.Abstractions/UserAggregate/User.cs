using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Modifications;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed record User
	: IIdentifiable<IUserId>,
	  IVersionable<UserVersion>,
	  IContinuous<User, UserEvent.Modified>
{
	public static Task<UserEvent.Registered> RegisterAsync(
		IWriteContext context,
		UserEmail userEmail,
		UserDisplayName displayName,
		Password password)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));
		Preconditions.RequiresNotNull(displayName, nameof(displayName));
		Preconditions.RequiresNotNull(password, nameof(password));

		return context.RegisterAsync(userEmail, displayName, password);
	}

	[return: NotNullIfNotNull("event")]
	public static User? After(UserEvent.Registered? @event)
	{
		if (@event is null)
		{
			return null;
		}

		if (!Eq.ValueSafe(@event.NewVersion, UserVersion.Initial))
		{
			throw new UserException(UserError.InvalidHistory);
		}

		return new User(
			@event.Id,
			@event.NewVersion,
			@event.Email,
			@event.Username,
			isActive: true,
			ImmutableList.Create<ResourceRequest>(),
			ImmutableHashSet.Create<Resource>()
		);
	}

	public User After(UserEvent.Modified? @event)
	{
		if (@event is null)
		{
			return this;
		}

		if (!Eq.ValueSafe(@event.Id, Id) || !@event.NewVersion.IsIncrementOf(Version))
		{
			throw new UserException(UserError.InvalidHistory);
		}

		return @event switch
		{
			UserEvent.Modified.ResourceRequested e => _AfterResourceRequested(e),
			UserEvent.Modified.ResourceAcquired e => _AfterResourceAcquired(e),
			UserEvent.Modified.ResourceDeleted e => _AfterResourceDeleted(e),
			UserEvent.Modified.PasswordChanged e => _AfterPasswordChanged(e),
			UserEvent.Modified.UsernameChanged e => _AfterUsernameChanged(e),
			UserEvent.Modified.Activated e => _AfterActivation(e),
			UserEvent.Modified.Deactivated e => _AfterDeactivation(e),
			_ => throw new UserException(UserError.InvalidHistory)
		};
	}

	private User(
		IUserId id,
		UserVersion version,
		UserEmail email,
		Username username,
		bool isActive,
		IImmutableList<ResourceRequest> resourceRequests,
		IImmutableSet<Resource> resources)
	{
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(version, nameof(version));
		Preconditions.RequiresNotNull(email, nameof(email));
		Preconditions.RequiresNotNull(username, nameof(username));
		Preconditions.RequiresNotNull(resourceRequests, nameof(resourceRequests));
		Preconditions.RequiresNotNull(resources, nameof(resources));

		Id = id;
		Version = version;
		Email = email;
		Username = username;
		IsActive = isActive;
		ResourceRequests = resourceRequests;
		Resources = resources;
	}

	public IUserId Id { get; }
	public UserVersion Version { get; private init; }

	public UserEmail Email { get; private init; }
	public Username Username { get; private init; }

	public bool IsActive { get; private init; }

	public IImmutableList<ResourceRequest> ResourceRequests { get; private init; }
	public IImmutableSet<Resource> Resources { get; private init; }

	public bool OwnResource(Resource resource)
	{
		Preconditions.RequiresNotNull(resource, nameof(resource));

		return Resources.Contains(resource);
	}

	public bool CanAccessResource(Resource resource)
	{
		Preconditions.RequiresNotNull(resource, nameof(resource));

		return IsActive && OwnResource(resource);
	}

	public Task<UserEvent.Modified.ResourceRequested> RequestResourceAsync(
		IWriteContext context,
		ResourceType type)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		if (!IsActive)
		{
			throw new UserException(UserError.Deactivated);
		}

		if (Resources.Any(e => e.Type == type))
		{
			throw new UserException(UserError.ResourceConflict);
		}

		if (ResourceRequests.Any(e => e.Type == type))
		{
			throw new UserException(UserError.ResourceConflict);
		}

		return context.RequestResourceAsync(this, type);
	}

	private User _AfterResourceRequested(UserEvent.Modified.ResourceRequested @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with
		{
			Version = @event.NewVersion,
			ResourceRequests = ResourceRequests.Add(@event.ResourceRequest)
		};
	}

	public Task<UserEvent.Modified.ResourceAcquired> AcquireResourceAsync(IWriteContext context, Resource resource)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		if (OwnResource(resource))
		{
			throw new UserException(UserError.ResourceAlreadyAcquired);
		}

		if (ResourceRequests.None(e => Eq.ValueSafe(e.Id, resource.RequestId)))
		{
			throw new UserException(UserError.ResourceNotRequested);
		}

		return context.AcquireResourceAsync(this, resource);
	}

	private User _AfterResourceAcquired(UserEvent.Modified.ResourceAcquired @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with
		{
			Version = @event.NewVersion,
			ResourceRequests = ResourceRequests.RemoveAll(e => Eq.ValueSafe(e.Id, @event.Resource.RequestId)),
			Resources = Resources.Add(@event.Resource)
		};
	}

	public Task<UserEvent.Modified.ResourceDeleted> DeleteResourceAsync(IWriteContext context, Resource resource)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		if (!IsActive)
		{
			throw new UserException(UserError.Deactivated);
		}

		if (!OwnResource(resource))
		{
			throw new UserException(UserError.DoesNotOwnResource);
		}

		return context.DeleteResourceAsync(this, resource);
	}

	private User _AfterResourceDeleted(UserEvent.Modified.ResourceDeleted @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with { Version = @event.NewVersion, Resources = Resources.Remove(@event.Resource) };
	}

	public Task<UserEvent.Modified.PasswordChanged> ChangePasswordAsync(
		IWriteContext context,
		Password currentPassword,
		Password newPassword)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(currentPassword, nameof(currentPassword));
		Preconditions.RequiresNotNull(newPassword, nameof(newPassword));

		if (!IsActive)
		{
			throw new UserException(UserError.Deactivated);
		}

		return context.ChangePasswordAsync(this, currentPassword, newPassword);
	}

	private User _AfterPasswordChanged(UserEvent.Modified.PasswordChanged @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with { Version = @event.NewVersion };
	}

	public async Task<UserEvent.Modified.UsernameChanged?> ChangeUsernameAsync(
		IWriteContext context,
		UserDisplayName displayName)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		if (!IsActive)
		{
			throw new UserException(UserError.Deactivated);
		}

		if (Eq.ValueSafe(Username.DisplayName, displayName))
		{
			return null;
		}

		return await context.ChangeUsernameAsync(this, displayName);
	}

	private User _AfterUsernameChanged(UserEvent.Modified.UsernameChanged @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with { Version = @event.NewVersion, Username = @event.NewUsername };
	}

	public Task<UserEvent.Modified.Activated> ActivateAsync(IWriteContext context, User user)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(user, nameof(user));

		if (IsActive)
		{
			throw new UserException(UserError.AlreadyActive);
		}

		return context.ActivateAsync(user);
	}

	private User _AfterActivation(UserEvent.Modified.Activated @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with { Version = @event.NewVersion, IsActive = true };
	}

	public Task<UserEvent.Modified.Deactivated> DeactivateAsync(IWriteContext context, User user)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(user, nameof(user));

		if (!IsActive)
		{
			throw new UserException(UserError.AlreadyDeactivated);
		}

		return context.DeactivateAsync(user);
	}

	private User _AfterDeactivation(UserEvent.Modified.Deactivated @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return this with { Version = @event.NewVersion, IsActive = false };
	}

	public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);

	public bool Equals(User? other) => ReferenceEquals(this, other);

	public static Task<User?> FindAsync(IReadContext context, IUserId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));

		return context.FindAsync(id);
	}

	public static async Task<User> GetAsync(IReadContext context, IUserId id)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(id, nameof(id));

		return await context.FindAsync(id).ConfigureAwait(false)
		       ?? throw new UserException(UserError.NotExists);
	}

	public static Task<User?> FindAsync(IReadContext context, Username username)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(username, nameof(username));

		return context.FindAsync(username);
	}

	public static async Task<User> GetAsync(IReadContext context, Username username)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(username, nameof(username));

		return await context.FindAsync(username).ConfigureAwait(false)
		       ?? throw new UserException(UserError.NotExists);
	}

	public static Task<User?> FindAsync(IReadContext context, UserEmail userEmail)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));

		return context.FindAsync(userEmail);
	}

	public static async Task<User> GetAsync(IReadContext context, UserEmail userEmail)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));

		return await context.FindAsync(userEmail).ConfigureAwait(false)
		       ?? throw new UserException(UserError.NotExists);
	}

	public interface IReadContext
	{
		protected internal Task<User?> FindAsync(IUserId id);
		protected internal Task<User?> FindAsync(Username username);
		protected internal Task<User?> FindAsync(UserEmail userEmail);

		protected static User FromHistory(IImmutableList<UserEvent> events)
		{
			Preconditions.RequiresNotNull(events, nameof(events));

			using var history = events.GetEnumerator();

			var created = history.MoveNext() ? history.Current as UserEvent.Registered : null;
			if (created is null)
			{
				throw new UserException(UserError.InvalidHistory);
			}

			var result = After(created);
			while (history.MoveNext())
			{
				var modified = history.Current as UserEvent.Modified
				               ?? throw new UserException(UserError.InvalidHistory);

				result = result.After(modified);
			}

			return result;
		}

		protected static User FromSnapshot(
			IUserId id,
			UserVersion version,
			UserEmail email,
			Username username,
			bool isActive,
			IImmutableList<ResourceRequest> resourceRequests,
			IImmutableSet<Resource> ownedResources)
		{
			Preconditions.RequiresNotNull(id, nameof(id));
			Preconditions.RequiresNotNull(version, nameof(version));
			Preconditions.RequiresNotNull(email, nameof(email));
			Preconditions.RequiresNotNull(username, nameof(username));
			Preconditions.RequiresNotNull(ownedResources, nameof(ownedResources));
			Preconditions.RequiresNotNullItems(resourceRequests, nameof(resourceRequests));
			Preconditions.RequiresNotNullItems(ownedResources, nameof(ownedResources));

			return new User(
				id,
				version,
				email,
				username,
				isActive,
				resourceRequests,
				ownedResources
			);
		}
	}

	public interface IWriteContext
	{
		protected internal Task<UserEvent.Registered> RegisterAsync(
			UserEmail userEmail,
			UserDisplayName displayName,
			Password password);

		protected internal Task<UserEvent.Modified.ResourceRequested> RequestResourceAsync(
			User user,
			ResourceType type);

		protected internal Task<UserEvent.Modified.ResourceAcquired> AcquireResourceAsync(
			User user,
			Resource resource);

		protected internal Task<UserEvent.Modified.ResourceDeleted> DeleteResourceAsync(
			User user,
			Resource resource);

		protected internal Task<UserEvent.Modified.PasswordChanged> ChangePasswordAsync(
			User user,
			Password currentPassword,
			Password newPassword);

		protected internal Task<UserEvent.Modified.UsernameChanged> ChangeUsernameAsync(
			User user,
			UserDisplayName displayName);

		protected internal Task<UserEvent.Modified.Activated> ActivateAsync(User user);
		protected internal Task<UserEvent.Modified.Deactivated> DeactivateAsync(User user);
	}
}