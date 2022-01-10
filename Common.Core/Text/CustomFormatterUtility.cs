using System.Globalization;
using Common.Core.Diagnostics;

namespace Common.Core.Text;

public static class CustomFormatterUtility
{
	public static string? HandleCustomFormatters<T>(this T? @this, string? format, IFormatProvider? formatProvider)
		=> (formatProvider?.GetFormat(typeof(T)) as ICustomFormatter)?.Format(format, @this, formatProvider);

	public static string HandleUnknownFormat(string? format, object? arg, CultureInfo? cultureInfo)
	{
		try
		{
			return arg switch
			{
				IFormattable formattable => formattable.ToString(format, cultureInfo),
				not null => arg.ToString().OrEmpty(),
				_ => string.Empty
			};
		}
		catch (Exception e)
		{
			throw Errors.InvalidFormat(format, e);
		}
	}
}