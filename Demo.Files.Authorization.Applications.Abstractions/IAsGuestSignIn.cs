using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Applications.Abstractions;

public interface IAsGuestSignIn
{
	public Task<Result> ExecuteAsync(string emailOrUsername, string password);

	public abstract class Result : ITypeEnum<Result, Result.Success, Result.Fail>
	{
		private Result()
		{
		}

		public sealed class Success : Result
		{
			public Success(User user)
			{
				Preconditions.RequiresNotNull(user, nameof(user));

				User = user;
			}

			public User User { get; }
		}

		public enum Error
		{
			InvalidIdentity,
			InvalidPassword,
			NotFound
		}

		public sealed class Fail : Result
		{
			public Fail(Error error)
			{
				Error = error;
			}

			public Error Error { get; }
		}
	}
}