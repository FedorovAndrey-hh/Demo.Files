using FluentAssertions;
using Xunit;

namespace Common.Core.Intervals;

public static class IntervalLimitTests
{
	public static class Compare
	{
		[Fact]
		public static void InclusiveWithSameValue_IsEqual()
		{
			var value = 0;
			var limit1 = IntervalLimit.Inclusive(value);

			var actual = limit1.IsEqual(value, Comparer<int>.Default);

			actual.Should().BeTrue();
		}

		[Fact]
		public static void ExclusiveWithSameValue_IsNotEqual()
		{
			var value = 0;
			var limit1 = IntervalLimit.Exclusive(value);

			var actual = limit1.IsEqual(value, Comparer<int>.Default);

			actual.Should().BeFalse();
		}

		[Fact]
		public static void InclusiveWithGraterValue_IsLess()
		{
			var value = 0;
			var graterValue = 1;
			var limit1 = IntervalLimit.Inclusive(value);

			var actual = limit1.IsLess(graterValue, Comparer<int>.Default);

			actual.Should().BeTrue();
		}

		[Fact]
		public static void ExclusiveWithGraterValue_IsLess()
		{
			var value = 0;
			var graterValue = 1;
			var limit1 = IntervalLimit.Exclusive(value);

			var actual = limit1.IsLess(graterValue, Comparer<int>.Default);

			actual.Should().BeTrue();
		}
	}
}