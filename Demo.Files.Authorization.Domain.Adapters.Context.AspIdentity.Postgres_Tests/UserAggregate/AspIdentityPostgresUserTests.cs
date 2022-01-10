using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Demo.Files.Authorization.Domain.Adapters.Context.AspIdentity.Postgres.UserAggregate;

public sealed class AspIdentityPostgresUserTests
	: UserTests,
	  IAsyncDisposable
{
	private static string _GetEnvironmentTestVariable(string name)
	{
		Preconditions.RequiresNotNull(name, nameof(name));

		var fullName = string.Concat(nameof(AspIdentityPostgresUserTests), "__", name);
		return Environment.GetEnvironmentVariable(fullName).NullIfEmpty()
		       ?? throw Errors.EnvironmentVariableNotFound(fullName);
	}

	public AspIdentityPostgresUserTests()
	{
		var serviceCollection = new ServiceCollection();

		serviceCollection
			.AddPostgresAspIdentityAuthorization(
				configure: options =>
				{
					options.Server = _GetEnvironmentTestVariable("address");
					options.Port = uint.Parse(_GetEnvironmentTestVariable("port"));
					options.Database = nameof(AspIdentityPostgresUserTests);
					options.Username = _GetEnvironmentTestVariable("username");
					options.Password = _GetEnvironmentTestVariable("password");
				}
			);

		_services = serviceCollection.BuildServiceProvider();

		var dbContext = _services.GetRequiredService<AuthorizationDbContext>();
		dbContext.Database.EnsureDeleted();
		dbContext.Database.Migrate();

		_transactionalContext = _services.GetRequiredService<IAuthorizationPersistenceContext>()
			.BeginTransactionAsync()
			.AsBlocking();
	}

	public async ValueTask DisposeAsync()
	{
		await _transactionalContext.DisposeAsync().ConfigureAwait(false);

		await _services.GetRequiredService<AuthorizationDbContext>()
			.Database.EnsureDeletedAsync()
			.ConfigureAwait(false);

		await _services.DisposeAsync().ConfigureAwait(false);
	}

	private readonly ServiceProvider _services;
	private readonly IAuthorizationTransactionalContext _transactionalContext;

	protected override User.IWriteContext Context => _transactionalContext.ForUser();

	protected override Task<Resource> HandleResourceRequestAsync(UserEvent.Modified.ResourceRequested @event)
	{
		Preconditions.RequiresNotNull(@event, nameof(@event));

		return (@event.ResourceRequest.Type switch
		{
			ResourceType.Storage => new Resource.Storage(
				StorageId.Of(@event.ResourceRequest.Id.RawLong()),
				@event.ResourceRequest.Id
			),
			_ => throw new ArgumentOutOfRangeException()
		}).AsTaskResult<Resource>();
	}

	[Fact]
	public override Task Register_WithValidParameters_CreatesUser() => base.Register_WithValidParameters_CreatesUser();

	[Fact]
	public override Task RequestResource_WithUniqueType_CanAcquireResource()
		=> base.RequestResource_WithUniqueType_CanAcquireResource();

	[Fact]
	public override Task Register_WithExistingEmail_ThrowsEmailConflictError()
		=> base.Register_WithExistingEmail_ThrowsEmailConflictError();

	[Fact]
	public override Task Register_WithExistingDisplayName_CreatesUsersWithUniqueUsernames()
		=> base.Register_WithExistingDisplayName_CreatesUsersWithUniqueUsernames();

	[Fact]
	public override Task ChangeUsername_Concurrently_ThrowsOutdatedError()
		=> base.ChangeUsername_Concurrently_ThrowsOutdatedError();
}