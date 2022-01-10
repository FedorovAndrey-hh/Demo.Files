using System.Security.Claims;
using System.Text.Json.Serialization;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Web;
using Demo.Files.Common.Contracts;
using Demo.Files.Common.Contracts.Authorization;
using Demo.Files.FilesManagement.Applications.Abstractions;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;
using Demo.Files.FilesManagement.Presentations.WebApi.Utility.Concurrency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.Files.FilesManagement.Presentations.WebApi.Storages;

[Authorize]
[ApiController]
[Route($"apis/v1/{RouteContextNames.FilesManagement}/")]
public sealed class StoragesController : ControllerBase
{
	private const string _pathDirectoryId = "directory-id";
	private const string _pathFileId = "file-id";

	public sealed class AddDirectoryRequest
	{
		[JsonConstructor]
		public AddDirectoryRequest(string name)
		{
			Preconditions.RequiresNotNull(name, nameof(name));

			Name = name;
		}

		public string Name { get; }
	}

	public sealed class AddDirectoryResponse
	{
		public AddDirectoryResponse(DirectoryId id)
		{
			Preconditions.RequiresNotNull(id, nameof(id));

			Id = id.RawLong();
		}

		public long Id { get; }
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(AddDirectoryResponse), StatusCodes.Status201Created)]
	[HttpPost("storages/my/directories")]
	public async Task<IActionResult> AddDirectoryAsync(
		[FromServices] IAddDirectory useCase,
		[IfMatchStorageVersion] StorageVersion? storageVersion,
		[FromBody] [BindRequired] AddDirectoryRequest request)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));

		var storageId = _GetStorageId();
		if (storageId is null)
		{
			return Unauthorized();
		}

		var (storage, directoryId) = await useCase.ExecuteAsync(
			storageId,
			storageVersion,
			request.Name
		);

		HttpContext.SetResponseStorageVersion(storage.Version);
		return Created(
			Request.GetCreatedAtLinkFromPost(directoryId.RawLong().ToString()),
			new AddDirectoryResponse(directoryId.Concrete())
		);
	}

	public sealed class RenameDirectoryRequest
	{
		[JsonConstructor]
		public RenameDirectoryRequest(string newName)
		{
			Preconditions.RequiresNotNull(newName, nameof(newName));

			NewName = newName;
		}

		public string NewName { get; }
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status304NotModified)]
	[HttpPost($"storages/my/directories/{{{_pathDirectoryId}}}:rename")]
	public async Task<IActionResult> RenameDirectoryAsync(
		[FromServices] IRenameDirectory useCase,
		[IfMatchStorageVersion] StorageVersion? storageVersion,
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryIdRaw,
		[FromBody] [BindRequired] RenameDirectoryRequest request)
	{
		Preconditions.RequiresNotNull(useCase, nameof(useCase));

		var storageId = _GetStorageId();
		if (storageId is null)
		{
			return Unauthorized();
		}

		var storage = await useCase.ExecuteAsync(
			storageId,
			storageVersion,
			DirectoryId.Of(directoryIdRaw),
			request.NewName
		);

		HttpContext.SetResponseStorageVersion(storage.Version);
		if (storageVersion is not null && Eq.ValueSafe(storageVersion, storage.Version))
		{
			return ActionResults.NotModified();
		}
		else
		{
			return NoContent();
		}
	}

	public sealed class RelocateDirectoryResponse
	{
		public RelocateDirectoryResponse(DirectoryId newId)
		{
			Preconditions.RequiresNotNull(newId, nameof(newId));

			NewId = newId.RawLong();
		}

		public long NewId { get; }
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(typeof(RelocateDirectoryResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status304NotModified)]
	[HttpPost($"storages/my/directories/{{{_pathDirectoryId}}}:relocate")]
	public async Task<IActionResult> RelocateDirectoryAsync(
		[FromServices] IRelocateDirectory useCase,
		[IfMatchStorageVersion] StorageVersion? storageVersion,
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryIdRaw)
	{
		var storageId = _GetStorageId();
		if (storageId is null)
		{
			return Unauthorized();
		}

		var (storage, newDirectoryId) = await useCase.ExecuteAsync(
			storageId,
			storageVersion,
			DirectoryId.Of(directoryIdRaw)
		);

		HttpContext.SetResponseStorageVersion(storage.Version);
		if (storageVersion is not null && Eq.ValueSafe(storageVersion, storage.Version))
		{
			return ActionResults.NotModified();
		}
		else
		{
			return Ok(new RelocateDirectoryResponse(newDirectoryId.Concrete()));
		}
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status304NotModified)]
	[HttpDelete($"storages/my/directories/{{{_pathDirectoryId}}}")]
	public async Task<IActionResult> RemoveDirectoryAsync(
		[FromServices] IRemoveDirectory useCase,
		[IfMatchStorageVersion] StorageVersion? storageVersion,
		[FromRoute(Name = _pathDirectoryId)] [BindRequired] long directoryIdRaw)
	{
		var storageId = _GetStorageId();
		if (storageId is null)
		{
			return Unauthorized();
		}

		var storage = await useCase.ExecuteAsync(
			storageId,
			storageVersion,
			DirectoryId.Of(directoryIdRaw)
		);

		HttpContext.SetResponseStorageVersion(storage.Version);
		if (storageVersion is not null && Eq.ValueSafe(storageVersion, storage.Version))
		{
			return ActionResults.NotModified();
		}
		else
		{
			return NoContent();
		}
	}

	[AllowAnonymous]
	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[HttpPost($"storages/my/directories/{{{_pathDirectoryId}}}/files")]
	public Task<IActionResult> AddFileAsync()
	{
		throw new NotImplementedException();
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[HttpPost($"storages/my/directories/{{{_pathDirectoryId}}}/files/{{{_pathFileId}}}:rename")]
	public Task<IActionResult> RenameFileAsync()
	{
		throw new NotImplementedException();
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[Produces(MediaTypeConstants.Application.Json)]
	[HttpPost($"storages/my/directories/{{{_pathDirectoryId}}}/files/{{{_pathFileId}}}:relocate")]
	public Task<IActionResult> RelocateFileAsync()
	{
		throw new NotImplementedException();
	}

	[Consumes(MediaTypeConstants.Application.Json)]
	[HttpDelete($"storages/my/directories/{{{_pathDirectoryId}}}/files/{{{_pathFileId}}}")]
	public Task<IActionResult> RemoveFileAsync()
	{
		throw new NotImplementedException();
	}

	private StorageId? _GetStorageId()
		=> long.TryParse(User.FindFirstValue(FilesClaimNames.Storage), out var rawId)
			? StorageId.Of(rawId)
			: null;
}