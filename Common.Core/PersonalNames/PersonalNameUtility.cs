using JetBrains.Annotations;

namespace Common.Core.PersonalNames;

public static class PersonalNameUtility
{
	[ContractAnnotation("fullNamePart: null => null; fullNamePart: notnull => notnull")]
	public static string? GetBestCompactNamePart(string? fullNamePart)
	{
		if (fullNamePart.IsNullOrEmpty())
		{
			return fullNamePart;
		}
		else if (fullNamePart.Length == 1 && fullNamePart[0].IsUpper())
		{
			return fullNamePart[0].ToString();
		}
		else if (fullNamePart.Length > 1 && fullNamePart[0].IsUpper())
		{
			return fullNamePart[0] + ".";
		}
		else
		{
			return fullNamePart;
		}
	}
}