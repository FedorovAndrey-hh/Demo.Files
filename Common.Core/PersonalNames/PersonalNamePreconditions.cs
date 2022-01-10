using Common.Core.Diagnostics;

namespace Common.Core.PersonalNames;

public static class PersonalNamePreconditions
{
	internal static void RequiresNotReservedPersonalNameTag(string? tag)
	{
		if (tag is not null)
		{
			Preconditions.Requires(
				!string.Equals(tag, nameof(PersonalName.GivenName), StringComparison.Ordinal),
				$"{nameof(PersonalName.GivenName)} is reserved. Use {nameof(PersonalName)}.{nameof(PersonalName.GivenName)} instead."
			);
			Preconditions.Requires(
				!string.Equals(tag, nameof(PersonalName.FamilyName), StringComparison.Ordinal),
				$"{nameof(PersonalName.FamilyName)} is reserved. Use {nameof(PersonalName)}.{nameof(PersonalName.FamilyName)} instead."
			);
		}
	}
}