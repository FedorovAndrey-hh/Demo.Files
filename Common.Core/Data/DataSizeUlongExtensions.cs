namespace Common.Core.Data;

public static class DataSizeUlongExtensions
{
	public static DataSize<ulong> Bits(this ulong @this) => DataSize.Bits(@this);
	public static DataSize<ulong> Kilobits(this ulong @this) => DataSize.Kilobits(@this);
	public static DataSize<ulong> Megabits(this ulong @this) => DataSize.Megabits(@this);
	public static DataSize<ulong> Gigabits(this ulong @this) => DataSize.Gigabits(@this);

	public static DataSize<ulong> Bytes(this ulong @this) => DataSize.Bytes(@this);
	public static DataSize<ulong> Kilobytes(this ulong @this) => DataSize.Kilobytes(@this);
	public static DataSize<ulong> Megabytes(this ulong @this) => DataSize.Megabytes(@this);
	public static DataSize<ulong> Gigabytes(this ulong @this) => DataSize.Gigabytes(@this);
}