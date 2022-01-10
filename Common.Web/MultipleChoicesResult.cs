using System.Collections.Immutable;
using Common.Core.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web;

public sealed class MultipleChoicesResult : StatusCodeResult
{
	public MultipleChoicesResult(IImmutableList<string> links)
		: base(StatusCodes.Status300MultipleChoices)
	{
		Preconditions.RequiresNotNull(links, nameof(links));

		_links = links;
	}

	private readonly IImmutableList<string> _links;

	public override void ExecuteResult(ActionContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));
		base.ExecuteResult(context);

		context.HttpContext.Response.Headers.Add(
			HttpHeaderConstants.Link,
			_links.Select(e => $"<{e}>; rel=\"alternate\"").ToArray()
		);
	}
}