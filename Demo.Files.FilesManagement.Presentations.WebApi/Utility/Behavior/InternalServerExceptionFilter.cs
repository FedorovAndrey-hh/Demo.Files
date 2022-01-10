using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Files.FilesManagement.Presentations.WebApi.Utility.Behavior;

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
		else if (context.Exception is StorageException storageException)
		{
			if (storageException.Error != StorageError.InvalidHistory
			    && storageException.Error != StorageError.LimitationsTotalSpaceTooLarge
			    && storageException.Error != StorageError.LimitationsTotalFileCountTooLarge
			    && storageException.Error != StorageError.LimitationsTotalSingleFileSizeTooLarge)
			{
				return;
			}

			var problemDetails = new ProblemDetails();
			problemDetails.SetTraceId(context.HttpContext);
			problemDetails.Title = WebApiResources.Error_InternalServerErrorTitle;
			problemDetails.Detail = storageException.Error switch
			{
				StorageError.InvalidHistory =>
					WebApiResources.StorageError_InvalidHistory,
				StorageError.LimitationsTotalSpaceTooLarge =>
					WebApiResources.StorageError_LimitationsTotalSpaceTooLarge,
				StorageError.LimitationsTotalFileCountTooLarge =>
					WebApiResources.StorageError_LimitationsTotalFileCountTooLarge,
				StorageError.LimitationsTotalSingleFileSizeTooLarge =>
					WebApiResources.StorageError_LimitationsTotalSingleFileSizeTooLarge,
				_ => throw Errors.UnsupportedEnumValue(storageException.Error)
			};
			context.Result = new ObjectResult(problemDetails);

			context.ExceptionHandled = true;
		}
	}
}