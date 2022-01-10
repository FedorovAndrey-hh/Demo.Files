using System.Collections.Immutable;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Modifications;
using Common.Core.Progressions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Domain.Adapters.Context.Testing.UserAggregate;

public sealed class UserContext
	: User.IWriteContext,
	  User.IReadContext
{
	private readonly object _lock = new();
	private readonly Dictionary<UserId, LinkedList<UserEvent>> _data = new();
	private readonly Dictionary<Username, UserId> _usernameIndex = new();
	private readonly Dictionary<UserEmail, UserId> _userEmailIndex = new();
	private readonly Dictionary<string, ushort> _usernameQuantifiers = new();

	private readonly IEnumerator<UserId> _userIdGenerator =
		new DelegateProgression<UserId>(
			UserId.Of(0),
			id => UserId.Of(id.Value + 1)
		).GetEnumerator();

	private readonly IEnumerator<ResourceRequestId> _resourceRequestGenerator =
		new DelegateProgression<ResourceRequestId>(
			ResourceRequestId.Of(0),
			id => ResourceRequestId.Of(id.Value + 1)
		).GetEnumerator();

	private Username _GenerateUsername(UserDisplayName displayName)
	{
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		if (_usernameQuantifiers.TryGetValue(displayName.AsString(), out var quantifier))
		{
			quantifier++;
		}
		else
		{
			quantifier = 1;
		}

		_usernameQuantifiers[displayName.AsString()] = quantifier;

		return Username.Create(displayName, quantifier);
	}

	Task<UserEvent.Registered> User.IWriteContext.RegisterAsync(
		UserEmail userEmail,
		UserDisplayName displayName,
		Password password)
	{
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));
		Preconditions.RequiresNotNull(displayName, nameof(displayName));
		Preconditions.RequiresNotNull(password, nameof(password));

		lock (_lock)
		{
			if (_userEmailIndex.ContainsKey(userEmail))
			{
				throw new UserException(UserError.EmailConflict);
			}

			var id = _userIdGenerator.Next();

			var @event = new UserEvent.Registered(
				id,
				UserVersion.Initial,
				userEmail,
				_GenerateUsername(displayName)
			);

			var history = new LinkedList<UserEvent>();

			history.AddLast(@event);

			_data[id] = history;
			_usernameIndex[@event.Username] = id;
			_userEmailIndex[@event.Email] = id;

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.ResourceRequested> User.IWriteContext.RequestResourceAsync(User user, ResourceType type)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.ResourceRequested(
				user.Id,
				user.Version.Increment(),
				ResourceRequest.Create(_resourceRequestGenerator.Next(), type)
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.ResourceAcquired> User.IWriteContext.AcquireResourceAsync(User user, Resource resource)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.ResourceAcquired(
				user.Id,
				user.Version.Increment(),
				resource
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.ResourceDeleted> User.IWriteContext.DeleteResourceAsync(User user, Resource resource)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(resource, nameof(resource));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.ResourceDeleted(
				user.Id,
				user.Version.Increment(),
				resource
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.PasswordChanged> User.IWriteContext.ChangePasswordAsync(
		User user,
		Password currentPassword,
		Password newPassword)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(currentPassword, nameof(currentPassword));
		Preconditions.RequiresNotNull(newPassword, nameof(newPassword));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.PasswordChanged(
				user.Id,
				user.Version.Increment()
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.UsernameChanged> User.IWriteContext.ChangeUsernameAsync(
		User user,
		UserDisplayName displayName)
	{
		Preconditions.RequiresNotNull(user, nameof(user));
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.UsernameChanged(
				user.Id,
				user.Version.Increment(),
				_GenerateUsername(displayName)
			);

			_usernameIndex[@event.NewUsername] = user.Id.Concrete();

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.Activated> User.IWriteContext.ActivateAsync(User user)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.Activated(
				user.Id,
				user.Version.Increment()
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	Task<UserEvent.Modified.Deactivated> User.IWriteContext.DeactivateAsync(User user)
	{
		Preconditions.RequiresNotNull(user, nameof(user));

		lock (_lock)
		{
			var @event = new UserEvent.Modified.Deactivated(
				user.Id,
				user.Version.Increment()
			);

			_Update(@event);

			return @event.AsTaskResult();
		}
	}

	private void _Update(UserEvent.Modified @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		if (!_data.TryGetValue(@event.Id.Concrete(), out var history))
		{
			throw new UserException(UserError.NotExists);
		}

		var lastEvent = history.Last?.Value;
		if (lastEvent is null || !@event.NewVersion.IsIncrementOf(lastEvent.NewVersion))
		{
			throw new UserException(UserError.Outdated);
		}

		history.AddLast(@event);
	}

	Task<User?> User.IReadContext.FindAsync(IUserId id)
	{
		Preconditions.RequiresNotNull(id, nameof(id));

		lock (_lock)
		{
			if (!_data.TryGetValue(id.Concrete(), out var history))
			{
				return Task.FromResult<User?>(null);
			}

			return User.IReadContext.FromHistory(history.ToImmutableList()).AsTaskResult<User?>();
		}
	}

	Task<User?> User.IReadContext.FindAsync(Username username)
	{
		Preconditions.RequiresNotNull(username, nameof(username));

		lock (_lock)
		{
			if (!_usernameIndex.TryGetValue(username, out var userId)
			    || !_data.TryGetValue(userId, out var history))
			{
				return Task.FromResult<User?>(null);
			}

			return User.IReadContext.FromHistory(history.ToImmutableList()).AsTaskResult<User?>();
		}
	}

	Task<User?> User.IReadContext.FindAsync(UserEmail userEmail)
	{
		Preconditions.RequiresNotNull(userEmail, nameof(userEmail));

		lock (_lock)
		{
			if (!_userEmailIndex.TryGetValue(userEmail, out var userId)
			    || !_data.TryGetValue(userId, out var history))
			{
				return Task.FromResult<User?>(null);
			}

			return User.IReadContext.FromHistory(history.ToImmutableList()).AsTaskResult<User?>();
		}
	}
}