using System.Text;
using Common.Core;
using Common.Core.Diagnostics;
using Demo.Files.PhysicalFiles.Domain.Abstractions.ContainerAggregate;
using FluentAssertions;

namespace Demo.Files.PhysicalFiles.Domain.Abstractions_Tests.ContainerAggregate;

public abstract class ContainerTests
{
	protected abstract Container.IContext Context { get; }

	#region Create

	public virtual async Task Create_WithoutParameters_CreatesContainer()
	{
		var container = await Container.CreateAsync(Context);

		container.Should().NotBeNull();
	}

	#endregion

	#region CreateFile

	public virtual async Task CreateFile_WithValidName_CreatesFile()
	{
		var container = await Container.CreateAsync(Context);

		var fileId = await container.CreateFileAsync(Context, _Utf8StringAsReadStreamProvider("text"));

		(await container.FileAsync(Context, fileId)).Should().NotBeNull();
	}

	#endregion

	#region ReadFile

	public virtual async Task ReadFile_Existing_ReturnsItsContent()
	{
		var expected = "text";

		var container = await Container.CreateAsync(Context);
		var fileId = await container.CreateFileAsync(Context, _Utf8StringAsReadStreamProvider(expected));
		await using var contentStream = await container.ReadFileAsync(Context, fileId);

		var actual = _Utf8StringFromStream(contentStream);

		actual.Should().Be(expected);
	}

	#endregion

	#region DeleteFile

	public virtual async Task DeleteFile_Existing_DeletesFile()
	{
		var container = await Container.CreateAsync(Context);

		var fileId = await container.CreateFileAsync(Context, _Utf8StringAsReadStreamProvider("text"));

		await container.DeleteFileAsync(Context, fileId);

		Action actual = () => container.FileAsync(Context, fileId).AsBlocking();

		actual.Should().Throw<ContainerException>();
	}

	#endregion

	private static Func<Stream> _Utf8StringAsReadStreamProvider(string text)
	{
		Preconditions.RequiresNotNull(text, nameof(text));

		return () => new MemoryStream(Encoding.UTF8.GetBytes(text));
	}

	private static string _Utf8StringFromStream(Stream stream)
	{
		Preconditions.RequiresNotNull(stream, nameof(stream));

		using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);

		return reader.ReadToEnd();
	}
}