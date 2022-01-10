using Common.Core.Diagnostics;
using Common.Core.Measurement;

namespace Common.Core.Data;

public sealed class DataSizeUnitMeasurers
{
	private static DataSizeUnitMeasurers? _cache;
	internal static DataSizeUnitMeasurers Create() => _cache ?? (_cache = new DataSizeUnitMeasurers());

	private DataSizeUnitMeasurers()
	{
	}

	public IMeasurer<ulong, DataSizeUnit> Ulong { get; } = new UlongMeasurer();

	private sealed class UlongMeasurer : IMeasurer<ulong, DataSizeUnit>
	{
		public ulong Measure(ulong sourceValue, DataSizeUnit sourceUnit, DataSizeUnit targetUnit)
		{
			Preconditions.RequiresNotNull(sourceUnit, nameof(sourceUnit));
			Preconditions.RequiresNotNull(targetUnit, nameof(targetUnit));

			if (Eq.ValueSafe(sourceUnit, targetUnit))
			{
				return sourceValue;
			}
			else if (Eq.StructSafe(sourceUnit.BitCount, targetUnit.BitCount))
			{
				return DecimalUnitPrefix.Measurers.Ulong.Measure(
					sourceValue,
					sourceUnit.UnitPrefix,
					targetUnit.UnitPrefix
				);
			}
			else if (Eq.ValueSafe<DecimalUnitPrefix>(sourceUnit.UnitPrefix, targetUnit.UnitPrefix))
			{
				return (sourceValue * sourceUnit.BitCount) / targetUnit.BitCount;
			}
			else
			{
				var sourceBits = sourceValue * sourceUnit.BitCount;
				var targetBits = DecimalUnitPrefix.Measurers.Ulong.Measure(
					sourceBits,
					sourceUnit.UnitPrefix,
					targetUnit.UnitPrefix
				);
				return targetBits / targetUnit.BitCount;
			}
		}
	}

	public IMeasurer<double, DataSizeUnit> Double { get; } = new DoubleMeasurer();

	private sealed class DoubleMeasurer : IMeasurer<double, DataSizeUnit>
	{
		public double Measure(double sourceValue, DataSizeUnit sourceUnit, DataSizeUnit targetUnit)
		{
			Preconditions.RequiresNotNull(sourceUnit, nameof(sourceUnit));
			Preconditions.RequiresNotNull(targetUnit, nameof(targetUnit));

			if (Eq.ValueSafe(sourceUnit, targetUnit))
			{
				return sourceValue;
			}
			else if (Eq.StructSafe(sourceUnit.BitCount, targetUnit.BitCount))
			{
				return DecimalUnitPrefix.Measurers.Double.Measure(
					sourceValue,
					sourceUnit.UnitPrefix,
					targetUnit.UnitPrefix
				);
			}
			else if (Eq.ValueSafe<DecimalUnitPrefix>(sourceUnit.UnitPrefix, targetUnit.UnitPrefix))
			{
				return sourceValue * ((double)sourceUnit.BitCount / targetUnit.BitCount);
			}
			else
			{
				var sourceBits = sourceValue * sourceUnit.BitCount;
				var targetBits = DecimalUnitPrefix.Measurers.Double.Measure(
					sourceBits,
					sourceUnit.UnitPrefix,
					targetUnit.UnitPrefix
				);
				return targetBits / targetUnit.BitCount;
			}
		}
	}
}