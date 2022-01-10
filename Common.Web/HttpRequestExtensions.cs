using Common.Core.Diagnostics;

namespace Common.Web;

public static class HttpRequestExtensions
{
	public static string GetCreatedAtLinkFromPost(this HttpRequest @this, string id)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(id, nameof(id));

		return $"{@this.Scheme}{Uri.SchemeDelimiter}{@this.Host}{@this.Path}/{Uri.EscapeDataString(id)}";
	}
}