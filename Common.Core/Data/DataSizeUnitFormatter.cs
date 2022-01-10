using System.Globalization;
using System.Resources;
using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Data;

/// <summary>
/// <list type="table">
///     <item>
///         <term><c>Fx</c></term>
///         <description>Full name,</description>
///     </item>
///     <item>
///         <term><c>Cx</c></term>
///         <description>Compact name,</description>
///     </item>
///     </list>
/// where <c>x</c> is a unit prefix format.
/// </summary>
public sealed class DataSizeUnitFormatter
	: ICustomFormatter,
	  IFormatProvider
{
	private static readonly Lazy<ResourceManager> _resources = new(
		() => typeof(DataSizeUnitResources).ToResourceManager()
	);

	public DataSizeUnitFormatter(
		CultureInfo cultureInfo,
		string? defaultFormat = null,
		string? defaultPrefixFormat = null)
	{
		Preconditions.RequiresNotNull(cultureInfo, nameof(cultureInfo));

		CultureInfo = cultureInfo.AsReadOnly();
		DefaultFormat = defaultFormat;
		DefaultPrefixFormat = defaultPrefixFormat;
	}

	public CultureInfo CultureInfo { get; }
	public string? DefaultFormat { get; }
	public string? DefaultPrefixFormat { get; }

	public object? GetFormat(Type? formatType) => Eq.Value(typeof(DataSizeUnit), formatType) ? this : null;

	public string Format(string? format, DataSizeUnit? arg, IFormatProvider? formatProvider)
	{
		if (arg is null)
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
		}

		var resourcePostfix = format?.LastOrDefault() switch
		{
			'F' => "Full",
			'C' => "Compact",
			_ => DefaultFormat
		};

		if (resourcePostfix is null)
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
		}

		var unitPrefixFormat = format.IsNullOrEmpty() || format.Length < 2 ? DefaultPrefixFormat : format[..^1];

		// ReSharper disable FormatStringProblem
		var unitPrefixFormatted = string.Format(formatProvider, "{0:" + unitPrefixFormat + "}", arg.UnitPrefix);
		// ReSharper restore FormatStringProblem

		var resourceCode = arg.BitCount switch
		{
			1 => "Bit",
			8 => "Byte",
			_ => null
		};

		var resourceName = resourceCode + resourcePostfix;

		var bitCountFormatted = resourceCode is null
			? arg.BitCount.ToString()
			: (_resources.Value.GetString(resourceName, CultureInfo)
			   ?? throw Errors.InvalidFormat(format, null));

		return unitPrefixFormatted + bitCountFormatted;
	}

	public string Format(string? format, object? arg, IFormatProvider? formatProvider)
	{
		if (arg is DataSizeUnit dataSizeUnit)
		{
			return Format(format, dataSizeUnit, formatProvider);
		}

		return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
	}
}