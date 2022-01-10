using System.Collections.Immutable;
using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.PersonalNames;

public sealed class PersonalName
	: IEquatable<PersonalName>,
	  IExternalFormattable<PersonalName>,
	  IFormattable
{
	private static IImmutableDictionary<string, string> _EmptyExtraName
		=> ImmutableSortedDictionary<string, string>.Empty;

	public static PersonalName Empty { get; } = new(null, null, _EmptyExtraName);

	public static PersonalName Create(string? givenName, string? familyName)
		=> new(givenName: givenName, familyName: familyName, _EmptyExtraName);

	public static PersonalNameStyledFormatters Formatters => PersonalNameStyledFormatters.Create();

	private PersonalName(string? givenName, string? familyName, IImmutableDictionary<string, string> extraNames)
	{
		GivenName = givenName;
		FamilyName = familyName;
		ExtraNames = extraNames;
	}

	public string? GivenName { get; }

	public PersonalName ChangeGivenName(string? value)
		=> new(givenName: value, familyName: FamilyName, extraNames: ExtraNames);

	public string? FamilyName { get; }

	public PersonalName ChangeFamilyName(string? value)
		=> new(givenName: GivenName, familyName: value, extraNames: ExtraNames);

	public bool IsEmpty => GivenName is null && FamilyName is null && ExtraNames.None();

	public IImmutableDictionary<string, string> ExtraNames { get; }

	public string? GetExtraName(string tag)
	{
		Preconditions.RequiresNotNull(tag, nameof(tag));
		PersonalNamePreconditions.RequiresNotReservedPersonalNameTag(tag);

		return ExtraNames.GetValueOrDefault(tag);
	}

	public PersonalName AddExtraName(string tag, string value)
	{
		Preconditions.RequiresNotNull(tag, nameof(tag));
		PersonalNamePreconditions.RequiresNotReservedPersonalNameTag(tag);
		Preconditions.RequiresNotNull(value, nameof(value));

		return ChangeExtraName(tag, value);
	}

	public PersonalName RemoveExtraName(string tag)
	{
		Preconditions.RequiresNotNull(tag, nameof(tag));
		PersonalNamePreconditions.RequiresNotReservedPersonalNameTag(tag);

		return ChangeExtraName(tag, null);
	}

	public PersonalName ChangeExtraName(string tag, string? value)
	{
		Preconditions.RequiresNotNull(tag, nameof(tag));
		PersonalNamePreconditions.RequiresNotReservedPersonalNameTag(tag);

		return new PersonalName(
			givenName: GivenName,
			familyName: FamilyName,
			extraNames: value is null ? ExtraNames.Remove(tag) : ExtraNames.SetItem(tag, value)
		);
	}

	public bool Equals(PersonalName? other, StringComparison comparisonType)
		=> ReferenceEquals(other, this)
		   || (other is not null
		       && string.Equals(other.GivenName, GivenName, comparisonType)
		       && string.Equals(other.FamilyName, FamilyName, comparisonType)
		       && other.ExtraNames.SequenceEqual(ExtraNames, new ExtraNamesComparator(comparisonType)));

	public bool Equals(PersonalName? other) => Equals(other, StringComparison.Ordinal);

	public override bool Equals(object? obj)
		=> ReferenceEquals(obj, this) || (obj is PersonalName personalName && Equals(personalName));

	public int GetHashCode(StringComparison comparisonType)
		=> HashCode.Combine(
			GivenName?.GetHashCode(comparisonType),
			FamilyName?.GetHashCode(comparisonType),
			ExtraNames.GetSequenceHashCode(new ExtraNamesComparator(comparisonType))
		);

	public override int GetHashCode() => GetHashCode(StringComparison.Ordinal);

	private sealed class ExtraNamesComparator : IEqualityComparer<KeyValuePair<string, string>>
	{
		public ExtraNamesComparator(StringComparison comparisonType)
		{
			_comparisonType = comparisonType;
		}

		private readonly StringComparison _comparisonType;

		public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
			=> string.Equals(x.Key, y.Key, StringComparison.Ordinal)
			   && string.Equals(x.Value, y.Value, _comparisonType);

		public int GetHashCode(KeyValuePair<string, string> obj)
			=> HashCode.Combine(
				obj.Key.GetHashCode(StringComparison.Ordinal),
				obj.Value.GetHashCode(_comparisonType)
			);
	}

	public string ToString(IExternalFormatter<PersonalName> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> this.HandleCustomFormatters(format, formatProvider) ?? ToString();

	public override string ToString() => $"PersonalName(Given name: `{GivenName}`,  Family name: `{FamilyName}`)";
}