using Common.Core.Diagnostics;
using Common.Core.Measurement;

namespace Common.Core.Data;

public sealed class DataSizeUnitQuantityEqualityComparers
{
	private static DataSizeUnitQuantityEqualityComparers? _cache;
	internal static DataSizeUnitQuantityEqualityComparers Create()
		=> _cache ?? (_cache = new DataSizeUnitQuantityEqualityComparers());

	private DataSizeUnitQuantityEqualityComparers()
	{
	}

	private static IEqualityComparer<IQuantity<ulong, DataSizeUnit>> _Ulong(DataSizeUnit precision)
	{
		Preconditions.RequiresNotNull(precision, nameof(precision));

		return DataSizeUnit.Measurers.Ulong.GetEqualityComparer(precision, EqualityComparer<ulong>.Default);
	}

	public IEqualityComparer<IQuantity<ulong, DataSizeUnit>> Ulong(DataSizeUnit precision) => _Ulong(precision);

	public IEqualityComparer<IQuantity<ulong, DataSizeUnit>> UlongBitPrecision { get; } = _Ulong(DataSizeUnit.Bit);

	public IEqualityComparer<IQuantity<ulong, DataSizeUnit>> UlongBytePrecision { get; }
		= _Ulong(DataSizeUnit.Byte);

	private static IEqualityComparer<IQuantity<double, DataSizeUnit>> _Double(DataSizeUnit precision)
	{
		Preconditions.RequiresNotNull(precision, nameof(precision));

		return DataSizeUnit.Measurers.Double.GetEqualityComparer(precision, EqualityComparer<double>.Default);
	}

	public IEqualityComparer<IQuantity<double, DataSizeUnit>> Double(DataSizeUnit precision) => _Double(precision);

	public IEqualityComparer<IQuantity<double, DataSizeUnit>> DoubleBitPrecision { get; }
		= _Double(DataSizeUnit.Bit);

	public IEqualityComparer<IQuantity<double, DataSizeUnit>> DoubleBytePrecision { get; }
		= _Double(DataSizeUnit.Byte);
}