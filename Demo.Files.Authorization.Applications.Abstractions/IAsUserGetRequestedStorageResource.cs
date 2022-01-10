using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Abstractions;

public interface IAsUserGetRequestedStorageResource
{
	public Task<Result> ExecuteAsync(IUserId userId, IResourceRequestId requestId);

	public abstract class Result : ITypeEnum<Result, Result.NotFound, Result.Requested, Result.Acquired>
	{
		private Result(User user)
		{
			Preconditions.RequiresNotNull(user, nameof(user));

			User = user;
		}

		public User User { get; }

		public sealed class NotFound : Result
		{
			public NotFound(User user)
				: base(user)
			{
			}
		}

		public sealed class Requested : Result
		{
			public Requested(User user)
				: base(user)
			{
			}
		}

		public sealed class Acquired : Result
		{
			public Acquired(User user, Resource.Storage storage)
				: base(user)
			{
				Preconditions.RequiresNotNull(storage, nameof(storage));

				Storage = storage;
			}

			public Resource.Storage Storage { get; }
		}
	}
}