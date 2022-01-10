using System.Collections.Immutable;
using System.Text;
using Common.Core;
using Common.Core.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Common.Web;

public static class ActionResults
{
	public static StatusCodeResult ResetContent() => new(StatusCodes.Status205ResetContent);

	public static StatusCodeResult MultipleChoices(IImmutableList<string> links)
	{
		Preconditions.RequiresNotNull(links, nameof(links));

		return new MultipleChoicesResult(links);
	}

	public static StatusCodeResult MultipleChoices(params string[] links)
	{
		Preconditions.RequiresNotNull(links, nameof(links));

		return new MultipleChoicesResult(ImmutableList.Create(links));
	}

	public static StatusCodeResult NotModified() => new(StatusCodes.Status304NotModified);

	public static ContentResult OkJson(string? json, Encoding? encoding = null)
	{
		var result = new ContentResult();
		result.Content = json;
		var contentType = MediaTypeHeaderValue.Parse(MediaTypeConstants.Application.Json);
		contentType.Encoding = encoding ?? contentType.Encoding;
		result.ContentType = contentType.ToString();
		result.StatusCode = StatusCodes.Status200OK;
		return result;
	}
}