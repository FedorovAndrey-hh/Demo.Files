using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;

internal static class UserDataUtility
{
	internal static void SetOriginalVersion(EntityEntry<UserData> entry, UserVersion version)
	{
		Preconditions.RequiresNotNull(entry, nameof(entry));

		entry.Property(e => e.Version).OriginalValue = version.Value;
	}
}