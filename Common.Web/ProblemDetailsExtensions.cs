using System.Diagnostics;
using Common.Core.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web;

public static class ProblemDetailsExtensions
{
	public static void SetTraceId(this ProblemDetails @this, HttpContext? context)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Extensions["traceId"] = Activity.Current?.Id ?? context?.TraceIdentifier;
	}
}