using System.Globalization;
using Common.Core.Diagnostics;
using Common.Core.Measurement;
using Common.Core.Text;

namespace Common.Core.Data;

/// <summary>
/// Format pattern: <c>u Vx Uy</c> where <c>x</c> is a data size unit name, <c>x</c> is a value format, <c>y</c> is a unit format.
/// </summary>
public sealed class DataSizeFormatter<TValue>
	: ICustomFormatter,
	  IFormatProvider
	where TValue : notnull
{
	public static string PrefixValue => "V";
	public static string PrefixUnit => "U";
	public static string DefaultValueUnitSeparator => "\u00A0";

	public DataSizeFormatter(
		CultureInfo cultureInfo,
		IMeasurer<TValue, DataSizeUnit> measurer,
		DataSizeUnit? defaultUnit = null,
		string? valueUnitSeparator = null,
		string? defaultValueFormat = null,
		string? defaultUnitFormat = null)
	{
		Preconditions.RequiresNotNull(cultureInfo, nameof(cultureInfo));
		Preconditions.RequiresNotNull(measurer, nameof(measurer));

		CultureInfo = cultureInfo.AsReadOnly();
		Measurer = measurer;
		DefaultUnit = defaultUnit;
		ValueUnitSeparator = valueUnitSeparator ?? DefaultValueUnitSeparator;
		DefaultValueFormat = defaultValueFormat;
		DefaultUnitFormat = defaultUnitFormat;
	}

	public CultureInfo CultureInfo { get; }
	public IMeasurer<TValue, DataSizeUnit> Measurer { get; }
	public DataSizeUnit? DefaultUnit { get; }
	public string ValueUnitSeparator { get; }
	public string? DefaultValueFormat { get; }
	public string? DefaultUnitFormat { get; }

	public object? GetFormat(Type? formatType) => Eq.Value(formatType, typeof(DataSize<TValue>)) ? this : null;

	public string Format(string? format, DataSize<TValue>? arg, IFormatProvider? formatProvider)
	{
		if (arg is null || (format is null && DefaultUnit is null))
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
		}

		var unit = DataSizeUnit.ByName(format?.BeforeFirst(' ').NullIfEmpty() ?? format)
		           ?? DefaultUnit ?? throw Errors.InvalidFormat(format, null);

		var valueFormat = format?.AfterFirst(PrefixValue).BeforeFirst(PrefixUnit).TrimEnd().NullIfEmpty()
		                  ?? DefaultValueFormat;
		var unitFormat = format?.AfterFirst(PrefixUnit).TrimEnd().NullIfEmpty() ?? DefaultUnitFormat;

		var measurer = Measurer;
		var measuredValue = arg.MeasureIn(unit, measurer);

		// ReSharper disable FormatStringProblem
		var formattedValue = string.Format(formatProvider, "{0:" + valueFormat + "}", measuredValue);
		var formattedUnit = string.Format(formatProvider, "{0:" + unitFormat + "}", unit);
		// ReSharper restore FormatStringProblem

		return CultureInfo.TextInfo.IsRightToLeft
			? formattedUnit + ValueUnitSeparator + formattedValue
			: formattedValue + ValueUnitSeparator + formattedUnit;
	}

	public string Format(string? format, object? arg, IFormatProvider? formatProvider)
	{
		if (arg is DataSize<TValue> dataSize)
		{
			return Format(format, dataSize, formatProvider);
		}

		return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
	}
}