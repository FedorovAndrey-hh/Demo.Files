using Common.Core.MathConcepts.GroupKind;

namespace Common.Core.Data;

public sealed class DataSizeMonoids
{
	private static DataSizeMonoids? _cache;
	internal static DataSizeMonoids Create() => _cache ?? (_cache = new DataSizeMonoids());

	private DataSizeMonoids()
	{
	}

	private static IMonoid<DataSize<ulong>> _UlongAddition(DataSizeUnit? precision)
		=> new DataSizeMonoid<ulong>(
			DataSize.Zero<ulong>(),
			StandardMonoids.UlongAddition,
			DataSizeUnit.Measurers.Ulong,
			precision
		);

	public IMonoid<DataSize<ulong>> UlongAddition(DataSizeUnit? precision) => _UlongAddition(precision);

	public IMonoid<DataSize<ulong>> UlongAdditionBytePrecision { get; } = _UlongAddition(DataSizeUnit.Byte);
}