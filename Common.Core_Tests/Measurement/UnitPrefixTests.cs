using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace Common.Core.Measurement;

public static class UnitPrefixTests
{
	public static class ConvertTo
	{
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public static TheoryData<DecimalUnitPrefix, double> SameUnitPrefix_ReturnsSameValue_TheoryData
			=> new()
			{
				{ DecimalUnitPrefix.None, 0d },
				{ DecimalUnitPrefix.None, -1d },
				{ DecimalUnitPrefix.None, 1d },

				{ DecimalUnitPrefix.Mega, 0d },
				{ DecimalUnitPrefix.Mega, -1d },
				{ DecimalUnitPrefix.Mega, 1d },

				{ DecimalUnitPrefix.Milli, 0d },
				{ DecimalUnitPrefix.Milli, -1d },
				{ DecimalUnitPrefix.Milli, 1d }
			};

		[Theory]
		[MemberData(nameof(SameUnitPrefix_ReturnsSameValue_TheoryData))]
		public static void ToSameUnitPrefix_ReturnsSameValue(DecimalUnitPrefix unitPrefix, double value)
		{
			var expected = value;

			var actual = DecimalUnitPrefix.Measurers.Double.Measure(value, unitPrefix, unitPrefix);

			actual.Should().Be(expected);
		}
	}
}