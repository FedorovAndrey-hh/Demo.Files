namespace Common.Core;

public static class NumberExtensions
{
	public static short Abs(this short @this) => Math.Abs(@this);
	public static int Abs(this int @this) => Math.Abs(@this);
	public static long Abs(this long @this) => Math.Abs(@this);
	public static ushort AbsTyped(this short @this) => (ushort)Math.Abs(@this);
	public static uint AbsTyped(this int @this) => (uint)Math.Abs(@this);
	public static ulong AbsTyped(this long @this) => (ulong)Math.Abs(@this);
	public static float Abs(this float @this) => Math.Abs(@this);
	public static double Abs(this double @this) => Math.Abs(@this);
	public static decimal Abs(this decimal @this) => Math.Abs(@this);

	public static uint Diff(this uint @this, uint other) => @this > other ? @this - other : other - @this;
	public static int Diff(this int @this, int other) => (@this > other ? @this - other : other - @this).Abs();
}