using Common.Core;
using Common.Core.Diagnostics;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public static class UserExtensions
{
	public static void AssertVersion(this User @this, UserVersion version)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(version, nameof(version));

		if (!Eq.ValueSafe(@this.Version, version))
		{
			throw new UserException(UserError.Outdated);
		}
	}
}