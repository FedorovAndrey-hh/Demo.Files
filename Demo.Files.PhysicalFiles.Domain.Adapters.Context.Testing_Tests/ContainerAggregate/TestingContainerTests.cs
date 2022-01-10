using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;
using Demo.Files.PhysicalFiles.Domain.Abstractions_Tests.ContainerAggregate;
using Xunit;

namespace Demo.Files.PhysicalFiles.Domain.Adapters.Context.Testing.ContainerAggregate;

public sealed class TestingContainerTests : ContainerTests
{
	protected override Container.IContext Context { get; } = new ContainerContext();

	[Fact]
	public override Task Create_WithoutParameters_CreatesContainer()
		=> base.Create_WithoutParameters_CreatesContainer();

	[Fact]
	public override Task CreateFile_WithValidName_CreatesFile() => base.CreateFile_WithValidName_CreatesFile();

	[Fact]
	public override Task ReadFile_Existing_ReturnsItsContent() => base.ReadFile_Existing_ReturnsItsContent();

	[Fact]
	public override Task DeleteFile_Existing_DeletesFile() => base.DeleteFile_Existing_DeletesFile();
}