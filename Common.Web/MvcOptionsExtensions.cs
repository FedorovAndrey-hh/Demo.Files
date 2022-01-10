using Common.Core.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web;

public static class MvcOptionsExtensions
{
	public static MvcOptions AddResponseStatusCodes(
		this MvcOptions @this,
		params int[] statusCodes)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(statusCodes, nameof(statusCodes));

		foreach (var statusCode in statusCodes)
		{
			@this.Filters.Add(new ProducesResponseTypeAttribute(statusCode));
		}

		return @this;
	}

	public static MvcOptions AddResponseType<T>(
		this MvcOptions @this,
		int statusCode)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Filters.Add(new ProducesResponseTypeAttribute(typeof(T), statusCode));

		return @this;
	}

	public static MvcOptions AddResponseType(
		this MvcOptions @this,
		Type type,
		int statusCode)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(type, nameof(type));

		@this.Filters.Add(new ProducesResponseTypeAttribute(type, statusCode));

		return @this;
	}
}