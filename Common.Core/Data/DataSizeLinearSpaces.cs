using Common.Core.MathConcepts.ModuleKind;
using Common.Core.MathConcepts.RingKind;

namespace Common.Core.Data;

public sealed class DataSizeLinearSpaces
{
	private static DataSizeLinearSpaces? _cache;
	internal static DataSizeLinearSpaces Create() => _cache ?? (_cache = new DataSizeLinearSpaces());

	private DataSizeLinearSpaces()
	{
	}

	private static ILinearSpace<DataSize<double>, double> _Double(DataSizeUnit? precision)
		=> new DataSizeLinearSpace<double>(
			DataSizeGroups.Create().DoubleAddition(precision),
			StandardFields.Double
		);

	public ILinearSpace<DataSize<double>, double> Double(DataSizeUnit? precision) => _Double(precision);

	public ILinearSpace<DataSize<double>, double> DoubleBitPrecision { get; } = _Double(DataSizeUnit.Bit);
	public ILinearSpace<DataSize<double>, double> DoubleBytePrecision { get; } = _Double(DataSizeUnit.Byte);
}