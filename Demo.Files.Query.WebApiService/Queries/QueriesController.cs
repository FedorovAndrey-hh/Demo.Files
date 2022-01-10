using System.Security.Claims;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Common.Contracts.Authorization;
using Demo.Files.Query.Views.Directories;
using Demo.Files.Query.Views.Files;
using Demo.Files.Query.Views.Storages;
using Demo.Files.Query.Views.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.Files.Query.WebApi.Queries;

[Authorize]
[ApiController]
[Route("apis/v1/files-management/")]
public sealed class QueriesController : ControllerBase
{
	private const string _pathDirectoryId = "directory-id";
	private const string _pathFileId = "file-id";

	[ProducesResponseType(StatusCodes.Status300MultipleChoices)]
	[HttpGet("users/me")]
	public IActionResult GetUser()
		=> ActionResults.MultipleChoices(Url.ActionLink(action: nameof(GetUserCompactAsync))!);

	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(UserCompact), StatusCodes.Status200OK)]
	[HttpGet("users/me/views/compact")]
	public async Task<IActionResult> GetUserCompactAsync([FromServices] IUserCompactViews views)
	{
		Preconditions.RequiresNotNull(views, nameof(views));

		var userId = _GetUserId();
		if (!userId.HasValue)
		{
			return Unauthorized();
		}

		return ActionResults.OkJson(await views.FindByIdAsync(userId.Value));
	}

	[ProducesResponseType(StatusCodes.Status300MultipleChoices)]
	[HttpGet("storages/my")]
	public IActionResult GetStorage()
		=> ActionResults.MultipleChoices(Url.ActionLink(action: nameof(GetStorageCompactAsync))!);

	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(StorageCompact), StatusCodes.Status200OK)]
	[HttpGet("storages/my/views/compact")]
	public async Task<IActionResult> GetStorageCompactAsync([FromServices] IStorageCompactViews views)
	{
		Preconditions.RequiresNotNull(views, nameof(views));

		var storageId = _GetStorageId();
		if (!storageId.HasValue)
		{
			return Unauthorized();
		}

		var result = await views.FindByIdAsync(storageId.Value);
		return result is not null ? ActionResults.OkJson(result) : NotFound();
	}

	[ProducesResponseType(StatusCodes.Status300MultipleChoices)]
	[HttpGet("storages/my/directories/all")]
	public IActionResult GetDirectories()
		=> ActionResults.MultipleChoices(Url.ActionLink(action: nameof(GetDirectoriesCompactAsync))!);

	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(IEnumerable<DirectoryCompact>), StatusCodes.Status200OK)]
	[HttpGet("storages/my/directories/all/views/compact")]
	public async Task<IActionResult> GetDirectoriesCompactAsync([FromServices] IDirectoryCompactViews views)
	{
		var storageId = _GetStorageId();
		if (!storageId.HasValue)
		{
			return Unauthorized();
		}

		return ActionResults.OkJson(await views.FindAllByStorageIdAsync(storageId.Value));
	}

	[ProducesResponseType(StatusCodes.Status300MultipleChoices)]
	[HttpGet($"storages/my/directories/{{{_pathDirectoryId}}}")]
	public IActionResult GetDirectory([FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryId)
		=> ActionResults.MultipleChoices(
			Url.ActionLink(
				action: nameof(GetDirectoryCompactAsync),
				values: new Dictionary<object, object>()
				{
					[_pathDirectoryId] = directoryId
				}
			)!
		);

	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(StorageCompact), StatusCodes.Status200OK)]
	[HttpGet($"storages/my/directories/{{{_pathDirectoryId}}}/views/compact")]
	public async Task<IActionResult> GetDirectoryCompactAsync(
		[FromServices] IDirectoryCompactViews views,
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryId)
	{
		var storageId = _GetStorageId();
		if (!storageId.HasValue)
		{
			return Unauthorized();
		}

		var result = await views.FindByIdAsync(storageId.Value, directoryId);
		return result is not null ? ActionResults.OkJson(result) : NotFound();
	}

	[ProducesResponseType(StatusCodes.Status300MultipleChoices)]
	[HttpGet($"storages/my/directories/{{{_pathDirectoryId}}}/files/all")]
	public IActionResult GetFiles(
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryId,
		[FromRoute(Name = _pathFileId)] [BindRequired] long fileId)
		=> ActionResults.MultipleChoices(
			Url.ActionLink(
				action: nameof(GetFilesCompactAsync),
				values: new Dictionary<object, object>()
				{
					[_pathDirectoryId] = directoryId,
					[_pathFileId] = fileId
				}
			)!,
			Url.ActionLink(
				action: nameof(GetFilesDetailAsync),
				values: new Dictionary<object, object>()
				{
					[_pathDirectoryId] = directoryId,
					[_pathFileId] = fileId
				}
			)!
		);

	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(IEnumerable<FileCompact>), StatusCodes.Status200OK)]
	[HttpGet($"storages/my/directories/{{{_pathDirectoryId}}}/files/all/views/compact")]
	public async Task<IActionResult> GetFilesCompactAsync(
		[FromServices] IFileCompactViews views,
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryId,
		[FromRoute(Name = _pathFileId)] [BindRequired] long fileId)
	{
		Preconditions.RequiresNotNull(views, nameof(views));

		var storageId = _GetStorageId();
		if (!storageId.HasValue)
		{
			return Unauthorized();
		}

		return ActionResults.OkJson(await views.FindByIdAsync(storageId.Value, directoryId, fileId));
	}

	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(IEnumerable<FileDetail>), StatusCodes.Status200OK)]
	[HttpGet($"storages/my/directories/{{{_pathDirectoryId}}}/files/all/views/detail")]
	public async Task<IActionResult> GetFilesDetailAsync(
		[FromServices] IFileDetailViews views,
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryId,
		[FromRoute(Name = _pathFileId)] [BindRequired] long fileId)
	{
		Preconditions.RequiresNotNull(views, nameof(views));

		var storageId = _GetStorageId();
		if (!storageId.HasValue)
		{
			return Unauthorized();
		}

		return ActionResults.OkJson(await views.FindByIdAsync(storageId.Value, directoryId, fileId));
	}

	private long? _GetUserId()
		=> long.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
			? id
			: null;

	private long? _GetStorageId()
		=> long.TryParse(User.FindFirstValue(FilesClaimNames.Storage), out var id)
			? id
			: null;
}