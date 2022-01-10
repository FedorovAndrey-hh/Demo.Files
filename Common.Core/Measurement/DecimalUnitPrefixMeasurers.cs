using Common.Core.Diagnostics;

namespace Common.Core.Measurement;

public sealed class DecimalUnitPrefixMeasurers
{
	private static DecimalUnitPrefixMeasurers? _cache;
	internal static DecimalUnitPrefixMeasurers Create() => _cache ?? (_cache = new DecimalUnitPrefixMeasurers());

	private DecimalUnitPrefixMeasurers()
	{
	}

	public IMeasurer<ulong, DecimalUnitPrefix> Ulong { get; } = new UlongMeasurer();

	private sealed class UlongMeasurer : IMeasurer<ulong, DecimalUnitPrefix>
	{
		public ulong Measure(ulong sourceValue, DecimalUnitPrefix sourceUnit, DecimalUnitPrefix targetUnit)
		{
			Preconditions.RequiresNotNull(sourceUnit, nameof(sourceUnit));
			Preconditions.RequiresNotNull(targetUnit, nameof(targetUnit));

			checked
			{
				var powerDiff = sourceUnit.Power - targetUnit.Power;
				return powerDiff switch
				{
					> 0 => sourceValue * MathExtensions.PowOf10Ulong(powerDiff.AbsTyped()),
					< 0 => sourceValue / MathExtensions.PowOf10Ulong(powerDiff.AbsTyped()),
					_ => sourceValue
				};
			}
		}
	}

	public IMeasurer<double, DecimalUnitPrefix> Double { get; } = new DoubleMeasurer();

	private sealed class DoubleMeasurer : IMeasurer<double, DecimalUnitPrefix>
	{
		public double Measure(double sourceValue, DecimalUnitPrefix sourceUnit, DecimalUnitPrefix targetUnit)
		{
			Preconditions.RequiresNotNull(sourceUnit, nameof(sourceUnit));
			Preconditions.RequiresNotNull(targetUnit, nameof(targetUnit));

			return sourceValue * Math.Pow(10, (double)sourceUnit.Power - targetUnit.Power);
		}
	}

	public IMeasurer<float, DecimalUnitPrefix> Float { get; } = new FloatMeasurer();

	private sealed class FloatMeasurer : IMeasurer<float, DecimalUnitPrefix>
	{
		public float Measure(float sourceValue, DecimalUnitPrefix sourceUnit, DecimalUnitPrefix targetUnit)
		{
			Preconditions.RequiresNotNull(sourceUnit, nameof(sourceUnit));
			Preconditions.RequiresNotNull(targetUnit, nameof(targetUnit));

			return (float)(sourceValue * Math.Pow(10, (double)sourceUnit.Power - targetUnit.Power));
		}
	}
}