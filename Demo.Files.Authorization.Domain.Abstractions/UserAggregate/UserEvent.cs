using Common.Core.Diagnostics;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public abstract class UserEvent
{
	private UserEvent(IUserId id, UserVersion newVersion)
	{
		Preconditions.RequiresNotNull(id, nameof(id));
		Preconditions.RequiresNotNull(newVersion, nameof(newVersion));

		Id = id;
		NewVersion = newVersion;
	}

	public IUserId Id { get; }
	public UserVersion NewVersion { get; }

	public sealed class Registered : UserEvent
	{
		public Registered(
			IUserId id,
			UserVersion newVersion,
			UserEmail email,
			Username username)
			: base(id, newVersion)
		{
			Preconditions.RequiresNotNull(email, nameof(email));
			Preconditions.RequiresNotNull(username, nameof(username));

			Email = email;
			Username = username;
		}

		public UserEmail Email { get; }
		public Username Username { get; }
	}

	public abstract class Modified : UserEvent
	{
		private Modified(
			IUserId id,
			UserVersion newVersion)
			: base(id, newVersion)
		{
		}

		public sealed class ResourceRequested : Modified
		{
			public ResourceRequested(IUserId id, UserVersion newVersion, ResourceRequest resourceRequest)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(resourceRequest, nameof(resourceRequest));

				ResourceRequest = resourceRequest;
			}

			public ResourceRequest ResourceRequest { get; }
		}

		public sealed class ResourceAcquired : Modified
		{
			public ResourceAcquired(IUserId id, UserVersion newVersion, Resource resource)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(resource, nameof(resource));

				Resource = resource;
			}

			public Resource Resource { get; }
		}

		public sealed class ResourceDeleted : Modified
		{
			public ResourceDeleted(IUserId id, UserVersion newVersion, Resource resource)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(resource, nameof(resource));

				Resource = resource;
			}

			public Resource Resource { get; }
		}

		public sealed class PasswordChanged : Modified
		{
			public PasswordChanged(IUserId id, UserVersion newVersion)
				: base(id, newVersion)
			{
			}
		}

		public sealed class UsernameChanged : Modified
		{
			public UsernameChanged(IUserId id, UserVersion newVersion, Username newUsername)
				: base(id, newVersion)
			{
				Preconditions.RequiresNotNull(newUsername, nameof(newUsername));

				NewUsername = newUsername;
			}

			public Username NewUsername { get; }
		}

		public sealed class Activated : Modified
		{
			public Activated(IUserId id, UserVersion newVersion)
				: base(id, newVersion)
			{
			}
		}

		public sealed class Deactivated : Modified
		{
			public Deactivated(IUserId id, UserVersion newVersion)
				: base(id, newVersion)
			{
			}
		}
	}
}