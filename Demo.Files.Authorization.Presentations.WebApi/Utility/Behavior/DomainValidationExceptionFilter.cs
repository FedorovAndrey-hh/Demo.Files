using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;

public sealed class DomainValidationExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		Preconditions.RequiresNotNull(context, nameof(context));

		if (context.ExceptionHandled)
		{
			return;
		}

		if (context.Exception is UserException exception)
		{
			if (exception.Error == UserError.InvalidHistory)
			{
				return;
			}

			(var errorMessage, context.HttpContext.Response.StatusCode) = exception.Error switch
			{
				UserError.NotExists => (
					WebApiResources.UserError_NotExists,
					StatusCodes.Status404NotFound),
				UserError.Outdated => (
					WebApiResources.UserError_Outdated,
					StatusCodes.Status412PreconditionFailed),
				UserError.UsernameConflict => (
					WebApiResources.UserError_UsernameConflict,
					StatusCodes.Status409Conflict),
				UserError.UsernameFormat => (
					WebApiResources.UserError_UsernameFormat,
					StatusCodes.Status422UnprocessableEntity),
				UserError.EmailConflict => (
					WebApiResources.UserError_EmailConflict,
					StatusCodes.Status409Conflict),
				UserError.Deactivated => (
					WebApiResources.UserError_Deactivated,
					StatusCodes.Status403Forbidden),
				UserError.ResourceConflict => (
					WebApiResources.UserError_ResourceConflict,
					StatusCodes.Status409Conflict),
				UserError.ResourceAlreadyAcquired => (
					WebApiResources.UserError_ResourceAlreadyAcquired,
					StatusCodes.Status409Conflict),
				UserError.ResourceNotRequested => (
					WebApiResources.UserError_ResourceNotRequested,
					StatusCodes.Status422UnprocessableEntity),
				UserError.DoesNotOwnResource => (
					WebApiResources.UserError_DoesNotOwnResource,
					StatusCodes.Status403Forbidden),
				UserError.AlreadyActive => (
					WebApiResources.UserError_AlreadyActive,
					StatusCodes.Status409Conflict),
				UserError.AlreadyDeactivated => (
					WebApiResources.UserError_AlreadyDeactivated,
					StatusCodes.Status409Conflict),
				UserError.PasswordEmpty => (
					WebApiResources.UserError_PasswordEmpty,
					StatusCodes.Status422UnprocessableEntity),
				UserError.PasswordInvalidLength => (
					WebApiResources.UserError_PasswordInvalidLength,
					StatusCodes.Status422UnprocessableEntity),
				UserError.PasswordUnsupportedCharacters => (
					WebApiResources.UserError_PasswordUnsupportedCharacters,
					StatusCodes.Status422UnprocessableEntity),
				UserError.EmailEmpty => (
					WebApiResources.UserError_EmailEmpty,
					StatusCodes.Status422UnprocessableEntity),
				UserError.EmailInvalidFormat => (
					WebApiResources.UserError_EmailInvalidFormat,
					StatusCodes.Status422UnprocessableEntity),
				UserError.EmailUnsupportedProvider => (
					WebApiResources.UserError_EmailUnsupportedProvider,
					StatusCodes.Status422UnprocessableEntity),
				UserError.EmailUnsupportedCharacters => (
					WebApiResources.UserError_EmailUnsupportedCharacters,
					StatusCodes.Status422UnprocessableEntity),
				UserError.DisplayNameEmpty => (
					WebApiResources.UserError_DisplayNameEmpty,
					StatusCodes.Status422UnprocessableEntity),
				UserError.DisplayNameTooLong => (
					WebApiResources.UserError_DisplayNameTooLong,
					StatusCodes.Status422UnprocessableEntity),
				UserError.DisplayNameUnsupportedCharacters => (
					WebApiResources.UserError_DisplayNameUnsupportedCharacters,
					StatusCodes.Status422UnprocessableEntity),
				UserError.DisplayNameInvalidFormat => (
					WebApiResources.UserError_DisplayNameInvalidFormat,
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