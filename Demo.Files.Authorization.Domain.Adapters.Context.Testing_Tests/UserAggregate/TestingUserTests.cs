using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.Authorization.Domain.Abstractions.UserAggregate;
using Xunit;

namespace Demo.Files.Authorization.Domain.Adapters.Context.Testing.UserAggregate;

public sealed class TestingUserTests : UserTests
{
	protected override User.IWriteContext Context { get; } = new UserContext();

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
	public override Task Register_WithValidParameters_CreatesUser()
		=> base.Register_WithValidParameters_CreatesUser();

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