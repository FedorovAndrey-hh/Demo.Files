using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;

public sealed class InternalServerExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		if (context.ExceptionHandled)
		{
			return;
		}

		if (context.Exception is ContractException contractException)
		{
			context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

			var problemDetails = new ProblemDetails();
			problemDetails.SetTraceId(context.HttpContext);
			problemDetails.Title = WebApiResources.Error_InternalServerErrorTitle;
			problemDetails.Detail = contractException.Message;
			context.Result = new ObjectResult(problemDetails);

			context.ExceptionHandled = true;
		}
		else if (context.Exception is UserException userException)
		{
			if (userException.Error != UserError.InvalidHistory)
			{
				return;
			}

			var problemDetails = new ProblemDetails();
			problemDetails.SetTraceId(context.HttpContext);
			problemDetails.Title = WebApiResources.Error_InternalServerErrorTitle;
			problemDetails.Detail = WebApiResources.UserError_InvalidHistory;
			context.Result = new ObjectResult(problemDetails);

			context.ExceptionHandled = true;
		}
	}
}