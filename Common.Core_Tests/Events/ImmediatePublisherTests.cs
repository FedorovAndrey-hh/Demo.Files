using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace Common.Core.Events;

public static class ImmediatePublisherTests
{
	public sealed record EventA;

	public sealed record EventB;

	public sealed record EventC;

	public static class Publish
	{
		[Fact]
		public static void WithDeclaredEventType_CallsHandlerOnce()
		{
			var expected = 1;

			var handler = new MockEventHandler<EventA>();

			var publisher = new ImmediateEventPublisher(ImmutableArray.Create<object>(handler));
			publisher.Publish(new EventA());

			var actual = handler.Events.Count;

			actual.Should().Be(expected);
		}

		[Fact]
		public static void WithUndeclaredEventType_DoNotCallHandler()
		{
			var expected = 0;

			var handler = new MockEventHandler<EventA>();

			var publisher = new ImmediateEventPublisher(ImmutableArray.Create<object>(handler));
			publisher.Publish(new EventB());

			var actual = handler.Events.Count;

			actual.Should().Be(expected);
		}
	}

	public static class PublishAsync
	{
		[Fact]
		public static async Task WithDeclaredEventType_CallsHandlerOnce()
		{
			var expected = 1;

			var handler = new MockAsyncEventHandler<EventA>();

			var publisher = new ImmediateAsyncEventPublisher(ImmutableArray.Create<object>(handler));
			await publisher.PublishAsync(new EventA());

			var actual = handler.Events.Count;

			actual.Should().Be(expected);
		}

		[Fact]
		public static async Task WithUndeclaredEventType_DoNotCallHandler()
		{
			var expected = 0;

			var handler = new MockAsyncEventHandler<EventA>();

			var publisher = new ImmediateAsyncEventPublisher(ImmutableArray.Create<object>(handler));
			await publisher.PublishAsync(new EventB());

			var actual = handler.Events.Count;

			actual.Should().Be(expected);
		}
	}
}