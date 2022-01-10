using System.Globalization;
using System.Resources;
using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Measurement;

/// <summary>
/// <list type="table">
///     <item>
///         <term><c>F</c></term>
///         <description>Full name,</description>
///     </item>
///     <item>
///         <term><c>C</c></term>
///         <description>Compact name.</description>
///     </item>
///     </list>
/// </summary>
public sealed class DecimalUnitPrefixFormatter
	: ICustomFormatter,
	  IFormatProvider
{
	private static readonly Lazy<ResourceManager> _resources = new(
		() => typeof(DecimalUnitPrefixResources).ToResourceManager()
	);

	public DecimalUnitPrefixFormatter(CultureInfo cultureInfo, string? defaultFormat = null)
	{
		Preconditions.RequiresNotNull(cultureInfo, nameof(cultureInfo));

		CultureInfo = cultureInfo.AsReadOnly();
		DefaultFormat = defaultFormat;
	}

	public CultureInfo CultureInfo { get; }
	public string? DefaultFormat { get; }

	public object? GetFormat(Type? formatType) => Eq.Value(formatType, typeof(DecimalUnitPrefix)) ? this : null;

	public string Format(string? format, DecimalUnitPrefix? arg, IFormatProvider? formatProvider)
	{
		if (arg is null)
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
		}

		var resourcePrefix = format switch
		{
			"F" => "Full",
			"C" => "Compact",
			_ => DefaultFormat
		};

		if (resourcePrefix is null)
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
		}

		var resourceName = resourcePrefix + arg.Power.ToString("00").Replace("-", "M");

		try
		{
			return _resources.Value.GetString(resourceName, CultureInfo)
			       ?? throw Errors.InvalidFormat(format, null);
		}
		catch (Exception e)
		{
			throw Errors.InvalidFormat(format, e);
		}
	}

	public string Format(string? format, object? arg, IFormatProvider? formatProvider)
	{
		if (arg is DecimalUnitPrefix unitPrefix)
		{
			return Format(format, unitPrefix, formatProvider);
		}

		return CustomFormatterUtility.HandleUnknownFormat(format, arg, CultureInfo);
	}
}