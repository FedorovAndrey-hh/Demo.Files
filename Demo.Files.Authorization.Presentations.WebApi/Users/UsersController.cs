using System.Security.Claims;
using System.Text.Json.Serialization;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Authorization.Applications.Abstractions;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Demo.Files.Authorization.Presentations.WebApi.Utility;
using Demo.Files.Authorization.Presentations.WebApi.Utility.Authentication;
using Demo.Files.Authorization.Presentations.WebApi.Utility.Behavior;
using Demo.Files.Common.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NJsonSchema.Annotations;

namespace Demo.Files.Authorization.Presentations.WebApi.Users;

[ApiController]
[Route($"apis/v1/{RouteContextNames.Authorization}")]
public sealed class UsersController : ControllerBase
{
	private const string _pathResourceRequestId = "resource-request-id";

	public sealed class RegisterRequest
	{
		[JsonConstructor]
		public RegisterRequest(string email, string displayName, string password)
		{
			Preconditions.RequiresNotNull(email, nameof(email));
			Preconditions.RequiresNotNull(displayName, nameof(displayName));
			Preconditions.RequiresNotNull(password, nameof(password));

			Email = email;
			DisplayName = displayName;
			Password = password;
		}

		public string Email { get; }
		public string DisplayName { get; }
		public string Password { get; }
	}

	public sealed class RegisterResponse
	{
		public RegisterResponse(UserId id, string accessToken)
		{
			Preconditions.RequiresNotNull(id, nameof(id));
			Preconditions.RequiresNotNull(accessToken, nameof(accessToken));

			Id = id;
			AccessToken = accessToken;
		}

		public UserId Id { get; set; }
		public string AccessToken { get; set; }
	}

	[AllowAnonymous]
	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponse))]
	[HttpPost("users:register")]
	public async Task<IActionResult> RegisterAsync(
		[FromServices] IAsGuestRegister useCase,
		[FromServices] IAccessTokenFactory accessTokenFactory,
		[FromBody] [BindRequired] RegisterRequest request)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));
		Preconditions.RequiresNotNull(accessTokenFactory, nameof(accessTokenFactory));
		Preconditions.RequiresNotNull(request, nameof(request));

		var user = await useCase
			.ExecuteAsync(
				request.Email,
				request.DisplayName,
				request.Password
			)
			.ConfigureAwait(false);
		var accessToken = await accessTokenFactory.CreateTokenAsync(user).ConfigureAwait(false);

		HttpContext.SetResponseUserVersion(user.Version);

		return Ok(new RegisterResponse(user.Id.Concrete(), accessToken));
	}

	public sealed class SignInRequest
	{
		[JsonConstructor]
		public SignInRequest(string emailOrUsername, string password)
		{
			Preconditions.RequiresNotNull(emailOrUsername, nameof(emailOrUsername));
			Preconditions.RequiresNotNull(password, nameof(password));

			EmailOrUsername = emailOrUsername;
			Password = password;
		}

		public string EmailOrUsername { get; }
		public string Password { get; }
	}

	public sealed class SignInResponse
	{
		public SignInResponse(UserId id, string accessToken)
		{
			Preconditions.RequiresNotNull(id, nameof(id));
			Preconditions.RequiresNotNull(accessToken, nameof(accessToken));

			Id = id;
			AccessToken = accessToken;
		}

		public UserId Id { get; set; }
		public string AccessToken { get; set; }
	}

	[AllowAnonymous]
	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SignInResponse))]
	[HttpPost("users:sign-in")]
	public async Task<IActionResult> SignInAsync(
		[FromServices] IAsGuestSignIn useCase,
		[FromServices] IAccessTokenFactory accessTokenFactory,
		[FromBody] [BindRequired] SignInRequest request)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));
		Preconditions.RequiresNotNull(accessTokenFactory, nameof(accessTokenFactory));
		Preconditions.RequiresNotNull(request, nameof(request));

		var result = await useCase
			.ExecuteAsync(request.EmailOrUsername, request.Password)
			.ConfigureAwait(false);
		switch (result)
		{
			case IAsGuestSignIn.Result.Fail fail:
				switch (fail.Error)
				{
					case IAsGuestSignIn.Result.Error.InvalidIdentity:
					{
						ModelState.AddModelError(
							nameof(request.EmailOrUsername),
							UsersControllerResources.SignIn_NoIdentity
						);

						return UnprocessableEntity(ModelState);
					}
					case IAsGuestSignIn.Result.Error.InvalidPassword:
						return Unauthorized();
					case IAsGuestSignIn.Result.Error.NotFound:
						return Unauthorized();
					default:
						throw Errors.UnsupportedEnumValue(fail.Error);
				}
			case IAsGuestSignIn.Result.Success success:
				HttpContext.SetResponseUserVersion(success.User.Version);
				return Ok(
					new SignInResponse(
						success.User.Id.Concrete(),
						await accessTokenFactory.CreateTokenAsync(success.User).ConfigureAwait(false)
					)
				);
			default:
				throw Errors.UnsupportedType(request.GetType());
		}
	}

	public sealed class ChangeUsernameRequest
	{
		[JsonConstructor]
		public ChangeUsernameRequest(string newDisplayName)
		{
			Preconditions.RequiresNotNull(newDisplayName, nameof(newDisplayName));

			NewDisplayName = newDisplayName;
		}

		public string NewDisplayName { get; }
	}

	public sealed class ChangeUsernameResponse
	{
		public ChangeUsernameResponse(Username username)
		{
			Preconditions.RequiresNotNull(username, nameof(username));

			Username = username;
		}

		public Username Username { get; }
	}

	[Authorize]
	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChangeUsernameResponse))]
	[ProducesResponseType(StatusCodes.Status304NotModified)]
	[HttpPost("users/me:change-username")]
	public async Task<IActionResult> ChangeUsernameAsync(
		[FromServices] IAsUserChangeUsername useCase,
		[IfMatchUserVersion] UserVersion? userVersion,
		[FromBody] [BindRequired] ChangeUsernameRequest request)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));
		Preconditions.RequiresNotNull(request, nameof(request));

		var userId = _GetUserId();

		var user = await useCase
			.ExecuteAsync(
				userId,
				userVersion,
				request.NewDisplayName
			)
			.ConfigureAwait(false);

		HttpContext.SetResponseUserVersion(user.Version);

		if (userVersion is not null && Eq.ValueSafe(user.Version, userVersion))
		{
			return ActionResults.NotModified();
		}
		else
		{
			return Ok(new ChangeUsernameResponse(user.Username));
		}
	}

	public sealed class ChangePasswordRequest
	{
		[JsonConstructor]
		public ChangePasswordRequest(string current, string @new)
		{
			Preconditions.RequiresNotNull(current, nameof(current));
			Preconditions.RequiresNotNull(@new, nameof(@new));

			Current = current;
			New = @new;
		}

		public string Current { get; }
		public string New { get; }
	}

	[Authorize]
	[Consumes(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status205ResetContent)]
	[HttpPost("users/me:change-password")]
	public async Task<IActionResult> ChangePasswordAsync(
		[FromServices] IAsUserChangePassword useCase,
		[IfMatchUserVersion] UserVersion? userVersion,
		[FromBody] [BindRequired] ChangePasswordRequest request)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));
		Preconditions.RequiresNotNull(request, nameof(request));

		var userId = _GetUserId();

		var user = await useCase
			.ExecuteAsync(
				userId,
				userVersion,
				request.Current,
				request.New
			)
			.ConfigureAwait(false);

		HttpContext.SetResponseUserVersion(user.Version);

		return ActionResults.ResetContent();
	}

	[Authorize]
	[ProducesResponseType(StatusCodes.Status202Accepted)]
	[HttpPost("users/me/resources/storage/request")]
	public async Task<IActionResult> RequestStorageResourceAsync(
		[FromServices] IAsUserRequestStorageResource useCase,
		[IfMatchUserVersion] UserVersion? userVersion)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));

		var userId = _GetUserId();

		var (user, resourceRequestId) = await useCase
			.ExecuteAsync(userId, userVersion)
			.ConfigureAwait(false);

		HttpContext.SetResponseUserVersion(user.Version);

		return AcceptedAtAction(
			nameof(GetStorageResourceRequestAsync),
			new Dictionary<string, object>()
			{
				[_pathResourceRequestId] = resourceRequestId.RawLong()
			},
			null
		);
	}

	[JsonSchemaFlatten]
	public sealed class RequestedResourceResponse : AsyncOperationResponse<StorageId, ProblemDetails>
	{
		public RequestedResourceResponse()
		{
		}

		public RequestedResourceResponse(StorageId result)
			: base(result)
		{
		}

		public RequestedResourceResponse(ProblemDetails error)
			: base(Preconditions.RequiresNotNull(error, nameof(error)))
		{
		}
	}

	[Authorize]
	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RequestedResourceResponse))]
	[ProducesResponseType(StatusCodes.Status304NotModified)]
	[HttpGet($"users/me/resources/storage/request/{{{_pathResourceRequestId}}}")]
	public async Task<IActionResult> GetStorageResourceRequestAsync(
		[FromServices] IAsUserGetRequestedStorageResource useCase,
		[IfNoneMatchUserVersion] UserVersion? userVersion,
		[FromRoute(Name = _pathResourceRequestId)] [BindRequired] long resourceRequestIdRaw)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));

		var userId = _GetUserId();

		var result = await useCase
			.ExecuteAsync(
				userId,
				ResourceRequestId.Of(resourceRequestIdRaw)
			)
			.ConfigureAwait(false);

		HttpContext.SetResponseUserVersion(result.User.Version);

		if (userVersion is not null && Eq.ValueSafe(userVersion, result.User.Version))
		{
			return ActionResults.NotModified();
		}

		switch (result)
		{
			case IAsUserGetRequestedStorageResource.Result.Acquired acquired:
				return Ok(new RequestedResourceResponse(acquired.Storage.Id.Concrete()));
			case IAsUserGetRequestedStorageResource.Result.NotFound:
				return NotFound();
			case IAsUserGetRequestedStorageResource.Result.Requested:
				return Ok(new RequestedResourceResponse());
			default:
				throw new ArgumentOutOfRangeException(nameof(result));
		}
	}

	private UserId _GetUserId() => UserId.Of(long.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
}