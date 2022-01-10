using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility;

public static class UserHttpContextExtensions
{
	public static void SetResponseUserVersion(this HttpContext @this, UserVersion version)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(version, nameof(version));

		@this.SetETagSingleValue(version.Value.ToString());
	}
}