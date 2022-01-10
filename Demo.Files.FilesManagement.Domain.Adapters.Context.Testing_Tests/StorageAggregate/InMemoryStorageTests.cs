using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Xunit;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.Testing.StorageAggregate;

public sealed class InMemoryStorageTests : StorageTests
{
	protected override Storage.IWriteContext Context { get; } = new StorageContext();

	protected override IPhysicalFileId GeneratePhysicalFile() => PhysicalFileId.Of(Guid.NewGuid());
		
	[Fact]
	public override Task Create_WithValidParameters_CreatesStorage()
		=> base.Create_WithValidParameters_CreatesStorage();

	[Fact]
	public override Task AddDirectory_WithValidParameters_CreatesEmptyDirectory()
		=> base.AddDirectory_WithValidParameters_CreatesEmptyDirectory();

	[Fact]
	public override Task AddDirectory_WithExistingName_ThrowsConflictError()
		=> base.AddDirectory_WithExistingName_ThrowsConflictError();

	[Fact]
	public override Task AddDirectory_InParallel_ThrowsOutdatedError()
		=> base.AddDirectory_InParallel_ThrowsOutdatedError();

	[Fact]
	public override Task AddFile_WithValidParameters_CreatesFile()
		=> base.AddFile_WithValidParameters_CreatesFile();

	[Fact]
	public override Task AddFile_WithExistingName_ThrowsStorageException()
		=> base.AddFile_WithExistingName_ThrowsStorageException();

	[Fact]
	public override Task MoveFile_ToAnotherDirectory_ChangesFileLocation()
		=> base.MoveFile_ToAnotherDirectory_ChangesFileLocation();
}