using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Files.FilesManagement.Presentations.WebApi.Utility.Behavior;

public sealed class DomainValidationExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		if (context.ExceptionHandled)
		{
			return;
		}

		if (context.Exception is StorageException exception)
		{
			if (exception.Error == StorageError.InvalidHistory
			    || exception.Error == StorageError.LimitationsTotalSpaceTooLarge
			    || exception.Error == StorageError.LimitationsTotalFileCountTooLarge
			    || exception.Error == StorageError.LimitationsTotalSingleFileSizeTooLarge)
			{
				return;
			}

			(var errorMessage, context.HttpContext.Response.StatusCode) = exception.Error switch
			{
				StorageError.NotExists => (
					WebApiResources.StorageError_NotExists,
					StatusCodes.Status404NotFound),
				StorageError.Outdated => (
					WebApiResources.StorageError_Outdated,
					StatusCodes.Status412PreconditionFailed),

				StorageError.ExceededLimitations => (
					WebApiResources.StorageError_ExceededLimitations,
					StatusCodes.Status403Forbidden),

				StorageError.DirectoryEmptyName => (
					WebApiResources.StorageError_DirectoryEmptyName,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.DirectoryNameWithUnsupportedCharacters => (
					WebApiResources.StorageError_DirectoryNameWithUnsupportedCharacters,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.DirectoryNameTooLarge => (
					WebApiResources.StorageError_DirectoryNameTooLarge,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.DirectoryNameConflict => (
					WebApiResources.StorageError_DirectoryNameConflict,
					StatusCodes.Status409Conflict),
				StorageError.DirectoryNotExists => (
					WebApiResources.StorageError_DirectoryNotExists,
					StatusCodes.Status422UnprocessableEntity),

				StorageError.FileEmptyName => (
					WebApiResources.StorageError_FileEmptyName,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.FileNameWithUnsupportedCharacters => (
					WebApiResources.StorageError_FileNameWithUnsupportedCharacters,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.FileNameTooLarge => (
					WebApiResources.StorageError_FileNameTooLarge,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.FileNameConflict => (
					WebApiResources.StorageError_FileNameConflict,
					StatusCodes.Status409Conflict),
				StorageError.FileNotExists => (
					WebApiResources.StorageError_FileNotExists,
					StatusCodes.Status422UnprocessableEntity),
				StorageError.FileIllegalMove => (
					WebApiResources.StorageError_FileIllegalMove,
					StatusCodes.Status422UnprocessableEntity),
				_ => (
					Enum.GetName(exception.Error)!,
					StatusCodes.Status400BadRequest)
			};

			context.ModelState.AddModelError(string.Empty, errorMessage);
			var details = new ValidationProblemDetails(context.ModelState);
			details.SetTraceId(context.HttpContext);
			context.Result = new ObjectResult(details);

			context.ExceptionHandled = true;
		}
	}
}