namespace Common.Core;

public static class CharExtensions
{
	public static bool IsUpper(this char @this) => char.IsUpper(@this);
	public static bool IsLower(this char @this) => char.IsLower(@this);
	public static bool IsLetterOrDigit(this char @this) => char.IsLetterOrDigit(@this);
	public static bool IsPunctuation(this char @this) => char.IsPunctuation(@this);
	public static bool IsControl(this char @this) => char.IsControl(@this);
	public static bool IsSymbol(this char @this) => char.IsSymbol(@this);
}