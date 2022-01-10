using Common.Core.Diagnostics;
using Common.Core.Measurement;

namespace Common.Core.Data;

public sealed class DataSizeUnitQuantityComparers
{
	private static DataSizeUnitQuantityComparers? _cache = new();
	internal static DataSizeUnitQuantityComparers Create() => _cache ?? (_cache = new DataSizeUnitQuantityComparers());

	private DataSizeUnitQuantityComparers()
	{
	}

	private static IComparer<IQuantity<ulong, DataSizeUnit>> _Ulong(DataSizeUnit precision)
	{
		Preconditions.RequiresNotNull(precision, nameof(precision));

		return DataSizeUnit.Measurers.Ulong.GetComparer(precision, Comparer<ulong>.Default);
	}

	public IComparer<IQuantity<ulong, DataSizeUnit>> Ulong(DataSizeUnit precision) => _Ulong(precision);

	public IComparer<IQuantity<ulong, DataSizeUnit>> _UlongBitPrecision { get; } = _Ulong(DataSizeUnit.Bit);

	public IComparer<IQuantity<ulong, DataSizeUnit>> UlongBytePrecision { get; } = _Ulong(DataSizeUnit.Byte);

	private static IComparer<IQuantity<double, DataSizeUnit>> _Double(DataSizeUnit precision)
	{
		Preconditions.RequiresNotNull(precision, nameof(precision));

		return DataSizeUnit.Measurers.Double.GetComparer(precision, Comparer<double>.Default);
	}

	public IComparer<IQuantity<double, DataSizeUnit>> Double(DataSizeUnit precision) => _Double(precision);

	public IComparer<IQuantity<double, DataSizeUnit>> DoubleBitPrecision { get; } = _Double(DataSizeUnit.Bit);

	public IComparer<IQuantity<double, DataSizeUnit>> DoubleBytePrecision { get; } = _Double(DataSizeUnit.Byte);
}