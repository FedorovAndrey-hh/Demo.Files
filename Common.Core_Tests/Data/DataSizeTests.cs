using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;
using Common.Core.MathConcepts;
using Common.Core.Measurement;
using FluentAssertions;
using Xunit;

namespace Common.Core.Data;

public static class DataSizeTests
{
	public static class Sum
	{
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public static TheoryData<DataSizeUnit, double, double> WithSameUnit_ReturnsSumOfValues_TheoryData
			=> new()
			{
				{ DataSizeUnit.Bit, 0d, 0d },
				{ DataSizeUnit.Bit, 1d, 1d },
				{ DataSizeUnit.Bit, 1d, -1d },
				{ DataSizeUnit.Bit, 1d, 2d },

				{ DataSizeUnit.Byte, 0d, 0d },
				{ DataSizeUnit.Byte, 1d, 1d },
				{ DataSizeUnit.Byte, 1d, -1d },
				{ DataSizeUnit.Byte, 1d, 2d },

				{ DataSizeUnit.Kilobit, 0d, 0d },
				{ DataSizeUnit.Kilobit, 1d, 1d },
				{ DataSizeUnit.Kilobit, 1d, -1d },
				{ DataSizeUnit.Kilobit, 1d, 2d },

				{ DataSizeUnit.Kilobyte, 0d, 0d },
				{ DataSizeUnit.Kilobyte, 1d, 1d },
				{ DataSizeUnit.Kilobyte, 1d, -1d },
				{ DataSizeUnit.Kilobyte, 1d, 2d }
			};

		[Theory]
		[MemberData(nameof(WithSameUnit_ReturnsSumOfValues_TheoryData))]
		public static void WithSameUnit_ReturnsSumOfValues(DataSizeUnit unit, double value1, double value2)
		{
			Preconditions.RequiresNotNull(unit, nameof(unit));

			var expected = value1 + value2;

			var actual = DataSize.Of(value1, unit)
				.Add(DataSize.Of(value2, unit), DataSize.LinearSpaces.DoubleBytePrecision)
				.MeasureIn(unit, DataSizeUnit.Measurers.Double);

			actual.Should().Be(expected);
		}
	}
}