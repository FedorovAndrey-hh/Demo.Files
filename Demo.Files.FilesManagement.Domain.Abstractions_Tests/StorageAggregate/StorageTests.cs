using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Modifications;
using FluentAssertions;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public abstract class StorageTests
{
	protected abstract Storage.IWriteContext Context { get; }

	#region Create

	public virtual async Task Create_WithValidParameters_CreatesStorage()
	{
		var storage = Storage.After(await Storage.CreateAsync(Context, Limitations.Maximum));

		storage.Should().NotBeNull();
	}

	#endregion

	#region AddDirectory

	public virtual async Task AddDirectory_WithValidParameters_CreatesEmptyDirectory()
	{
		var storage = Storage.After(await Storage.CreateAsync(Context, Limitations.Maximum));

		var directoryName = DirectoryName.Create("directory");
		var directoryCreated = await storage.AddDirectoryAsync(Context, directoryName);
		storage = storage.After(directoryCreated);

		storage.FindDirectory(directoryName).Should().NotBeNull();
		storage.Metrics.Should().Be(Metrics.Empty);

		var directory = storage.Directory(directoryCreated.DirectoryId);
		directory.Name.Should().Be(directoryName);
		directory.IsEmpty().Should().BeTrue();
		directory.Metrics.Should().Be(Metrics.Empty);
	}

	public virtual async Task AddDirectory_WithExistingName_ThrowsConflictError()
	{
		var storage = Storage.After(await Storage.CreateAsync(Context, Limitations.Maximum));

		var directoryName = DirectoryName.Create("directory");

		storage = await storage.AfterActionAsync(e => e.AddDirectoryAsync(Context, directoryName));

		StorageError? actual = null;
		try
		{
			await storage.AddDirectoryAsync(Context, directoryName);
		}
		catch (StorageException e)
		{
			actual = e.Error;
		}

		actual.Should().Be(StorageError.DirectoryNameConflict);
	}

	public virtual async Task AddDirectory_InParallel_ThrowsOutdatedError()
	{
		var created = await Storage.CreateAsync(Context, Limitations.Maximum);

		var storage1 = Storage.After(created);
		var storage2 = Storage.After(created);

		await storage1.AddDirectoryAsync(Context, DirectoryName.Create("Directory1"));

		StorageError? error = null;
		try
		{
			await storage2.AddDirectoryAsync(Context, DirectoryName.Create("Directory2"));
		}
		catch (StorageException e)
		{
			error = e.Error;
		}

		error.Should().Be(StorageError.Outdated);
	}

	#endregion

	protected abstract IPhysicalFileId GeneratePhysicalFile();

	#region AddFile

	public virtual async Task AddFile_WithValidParameters_CreatesFile()
	{
		var (storage, directoryIds) = await _CreateStorageWithDirectoriesAsync(
			Limitations.Create(1ul.Bytes(), 1, 1ul.Bytes()),
			DirectoryName.Create("directory")
		);
		var directoryId = directoryIds[0];

		var fileName = FileName.Create("file");
		var fileSize = 0ul.Bytes();
		var fileAdded = await storage
			.AddFileAsync(
				Context,
				directoryId,
				GeneratePhysicalFile(),
				fileName,
				fileSize
			);
		storage = storage.After(fileAdded);

		storage.Metrics.Should().Be(Metrics.OfSingleFile(fileSize));

		var directory = storage.Directory(directoryId);
		directory.Metrics.Should().Be(Metrics.OfSingleFile(fileSize));
		directory.FindFile(fileName).Should().NotBeNull();

		var file = directory.File(fileAdded.FileId);
		file.Name.Should().Be(fileName);
		file.Size.Should().Be(fileSize);
	}

	public virtual async Task AddFile_WithExistingName_ThrowsStorageException()
	{
		var (storage, directoryIds) = await _CreateStorageWithDirectoriesAsync(
			Limitations.Create(1ul.Bytes(), 1, 1ul.Bytes()),
			DirectoryName.Create("directory")
		);
		var directoryId = directoryIds[0];

		var fileName = FileName.Create("file");

		storage = await storage.AfterActionAsync(
			e => e.AddFileAsync(
				Context,
				directoryId,
				GeneratePhysicalFile(),
				fileName,
				DataSize.Zero<ulong>()
			)
		);

		Action actual = () => storage.AddFileAsync(
				Context,
				directoryId,
				GeneratePhysicalFile(),
				fileName,
				DataSize.Zero<ulong>()
			)
			.AsBlocking();

		actual.Should().Throw<StorageException>();
	}

	#endregion

	#region MoveFile

	public virtual async Task MoveFile_ToAnotherDirectory_ChangesFileLocation()
	{
		var (storage, directoryIds) = await _CreateStorageWithDirectoriesAsync(
			Limitations.Create(1ul.Bytes(), 1, 1ul.Bytes()),
			DirectoryName.Create("source_directory"),
			DirectoryName.Create("destination_directory")
		);
		var sourceDirectoryId = directoryIds[0];
		var destinationDirectoryId = directoryIds[1];

		var fileName = FileName.Create("file");
		var fileAdded
			= await storage.AddFileAsync(
				Context,
				sourceDirectoryId,
				GeneratePhysicalFile(),
				fileName,
				DataSize.Zero<ulong>()
			);
		var sourceFileId = fileAdded.FileId;
		storage = storage.After(fileAdded);

		storage.Directory(sourceDirectoryId).FindFile(sourceFileId).Should().NotBeNull();
		storage.Directory(destinationDirectoryId).FindFile(fileName).Should().BeNull();

		var fileMoved = await storage.MoveFileAsync(
			Context,
			sourceDirectoryId,
			sourceFileId,
			destinationDirectoryId
		);
		var newFileId = fileMoved.NewFileId;
		storage = storage.After(fileMoved);

		storage.Directory(sourceDirectoryId).FindFile(sourceFileId).Should().BeNull();
		storage.Directory(sourceDirectoryId).FindFile(fileName).Should().BeNull();
		storage.Directory(destinationDirectoryId).FindFile(newFileId).Should().NotBeNull();
	}

	#endregion

	private async Task<(Storage, IDirectoryId[])> _CreateStorageWithDirectoriesAsync(
		Limitations limitations,
		params DirectoryName[] directoryNames)
	{
		Preconditions.RequiresNotNull(directoryNames, nameof(directoryNames));

		var storage = Storage.After(await Storage.CreateAsync(Context, limitations));

		var directoryIds = new IDirectoryId[directoryNames.Length];
		for (var i = 0; i < directoryNames.Length; i++)
		{
			var directoryAdded = await storage.AddDirectoryAsync(Context, directoryNames[i]);
			directoryIds[i] = directoryAdded.DirectoryId;

			storage = storage.After(directoryAdded);
		}

		return (storage, directoryIds);
	}
}