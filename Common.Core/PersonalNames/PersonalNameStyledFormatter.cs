using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.PersonalNames;

public sealed class PersonalNameStyledFormatter : IExternalFormatter<PersonalName>
{
	public PersonalNameStyledFormatter(PersonalNameOrder order, PersonalNameStyle style)
	{
		Order = order;
		Style = style;
	}

	public PersonalNameOrder Order { get; }
	public PersonalNameStyle Style { get; }

	public string Format(PersonalName data)
	{
		Preconditions.RequiresNotNull(data, nameof(data));

		var (givenName, familyName, extraName) = Style switch
		{
			PersonalNameStyle.Compact => (
				PersonalNameUtility.GetBestCompactNamePart(data.GivenName),
				data.FamilyName,
				PersonalNameUtility.GetBestCompactNamePart(data.GetFirstDirectAncestorName())),
			PersonalNameStyle.Full => (
				data.GivenName,
				data.FamilyName,
				data.GetFirstDirectAncestorName()),
			_ => throw Contracts.UnreachableThrow()
		};

		string?[] parts = Order switch
		{
			PersonalNameOrder.Western => new[] { givenName, familyName, extraName },
			PersonalNameOrder.Eastern => new[] { familyName, givenName, extraName },
			_ => throw Contracts.UnreachableThrow()
		};

		return string.Join(Characters.NonBreakingSpace, parts.Where(e => !e.IsNullOrEmpty()));
	}
}