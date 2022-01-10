using Common.Core;
using Common.Core.Diagnostics;
using Common.EntityFramework.Postgres;
using Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;
using Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.StorageAggregate;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Demo.Files.FilesManagement.Domain.Adapters.Context.EntityFramework.Postgres.StorageAggregate;

public sealed class EntityFrameworkPostgresStorageTests
	: StorageTests,
	  IAsyncDisposable
{
	private static string _GetEnvironmentTestVariable(string name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		var fullName = string.Concat(nameof(EntityFrameworkPostgresStorageTests), "__", name);
		return Environment.GetEnvironmentVariable(fullName).NullIfEmpty()
		       ?? throw Errors.EnvironmentVariableNotFound(fullName);
	}

	public EntityFrameworkPostgresStorageTests()
	{
		_dbContext = new PostgresFilesManagementDbContext(
			new PostgresConnectionInfo(
				_GetEnvironmentTestVariable("address"),
				uint.Parse(_GetEnvironmentTestVariable("port")),
				nameof(EntityFrameworkPostgresStorageTests),
				_GetEnvironmentTestVariable("username"),
				_GetEnvironmentTestVariable("password")
			)
		);

		_dbContext.Database.EnsureDeleted();
		_dbContext.Database.Migrate();

		_transactionalContext = _dbContext.BeginTransactionAsync().AsBlocking();
	}

	public async ValueTask DisposeAsync()
	{
		await _transactionalContext.DisposeAsync();

		await _dbContext.Database.EnsureDeletedAsync();
		await _dbContext.DisposeAsync();
	}

	private readonly FilesManagementDbContext _dbContext;
	private readonly IFilesManagementTransactionalWriteContext _transactionalContext;

	protected override Storage.IWriteContext Context => _transactionalContext.ForStorage();

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