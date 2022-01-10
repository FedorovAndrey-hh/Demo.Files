namespace Common.Core.Data;

public static class DataSizeDoubleExtensions
{
	public static DataSize<double> Bits(this double @this) => DataSize.Bits(@this);
	public static DataSize<double> Kilobits(this double @this) => DataSize.Kilobits(@this);
	public static DataSize<double> Megabits(this double @this) => DataSize.Megabits(@this);
	public static DataSize<double> Gigabits(this double @this) => DataSize.Gigabits(@this);

	public static DataSize<double> Bytes(this double @this) => DataSize.Bytes(@this);
	public static DataSize<double> Kilobytes(this double @this) => DataSize.Kilobytes(@this);
	public static DataSize<double> Megabytes(this double @this) => DataSize.Megabytes(@this);
	public static DataSize<double> Gigabytes(this double @this) => DataSize.Gigabytes(@this);
}