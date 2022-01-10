using Common.Core.MathConcepts.GroupKind;

namespace Common.Core.Data;

public sealed class DataSizeGroups
{
	private static DataSizeGroups? _cache;

	internal static DataSizeGroups Create() => _cache ?? (_cache = new DataSizeGroups());

	private DataSizeGroups()
	{
	}

	private static IGroup<DataSize<double>> _DoubleAddition(DataSizeUnit? precision)
		=> new DataSizeGroup<double>(
			DataSize.Zero<double>(),
			StandardGroups.DoubleAddition,
			DataSizeUnit.Measurers.Double,
			precision
		);

	public IGroup<DataSize<double>> DoubleAddition(DataSizeUnit? precision) => _DoubleAddition(precision);

	public IGroup<DataSize<double>> DoubleAdditionBitPrecision { get; } = _DoubleAddition(DataSizeUnit.Bit);
	public IGroup<DataSize<double>> DoubleAdditionBytePrecision { get; } = _DoubleAddition(DataSizeUnit.Byte);
}