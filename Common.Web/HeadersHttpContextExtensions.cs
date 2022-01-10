using Common.Core.Diagnostics;
using Microsoft.Extensions.Primitives;

namespace Common.Web;

public static class HeadersHttpContextExtensions
{
	public static void SetETagSingleValue(this HttpContext @this, string etagSingleValue)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(etagSingleValue, nameof(etagSingleValue));

		@this.Response.Headers.Add(HttpHeaderConstants.ETag, new StringValues("\"" + etagSingleValue + "\""));
	}
		
	public static void SetETag(this HttpContext @this, string etag)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(etag, nameof(etag));

		@this.Response.Headers.Add(HttpHeaderConstants.ETag, new StringValues(etag));
	}
}