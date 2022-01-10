using System.Collections.Immutable;
using Common.Core.Text;

namespace Common.Core.PersonalNames;

public sealed class PersonalNameFormatter
	: ICustomFormatter,
	  IFormatProvider
{
	public static char NamePartFullMarker => '~';
	public static char NamePartCompactMarker => '-';

	public PersonalNameFormatter(string? defaultFormat = null)
	{
		DefaultFormat = defaultFormat;
	}

	public string? DefaultFormat { get; }

	public object? GetFormat(Type? formatType) => Eq.Value(formatType, typeof(PersonalName)) ? this : null;

	public string Format(string? format, PersonalName? arg, IFormatProvider? formatProvider)
	{
		if (arg is null)
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, null);
		}

		var result = format ?? DefaultFormat;

		if (result is null)
		{
			return CustomFormatterUtility.HandleUnknownFormat(format, arg, null);
		}

		var parts = result.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
		foreach (var part in parts)
		{
			var isFullPart = part.StartsWith(NamePartFullMarker);
			var isCompactPart = part.StartsWith(NamePartCompactMarker);

			if (isFullPart || isCompactPart)
			{
				var tag = part.Substring(1);
				var name = tag switch
				{
					nameof(PersonalName.GivenName) => arg.GivenName,
					nameof(PersonalName.FamilyName) => arg.FamilyName,
					_ => arg.ExtraNames.GetValueOrDefault(tag)
				};
				if (isCompactPart)
				{
					name = PersonalNameUtility.GetBestCompactNamePart(name);
				}

				result = result.Replace(part, name);
			}
		}

		return result;
	}

	public string Format(string? format, object? arg, IFormatProvider? formatProvider)
	{
		if (arg is PersonalName personalName)
		{
			return Format(format, personalName, formatProvider);
		}

		return CustomFormatterUtility.HandleUnknownFormat(format, arg, null);
	}
}