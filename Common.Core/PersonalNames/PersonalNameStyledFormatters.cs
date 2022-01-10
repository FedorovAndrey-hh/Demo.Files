namespace Common.Core.PersonalNames;

public sealed class PersonalNameStyledFormatters
{
	private static PersonalNameStyledFormatters? _cache;
	internal static PersonalNameStyledFormatters Create() => _cache ?? (_cache = new PersonalNameStyledFormatters());

	private PersonalNameStyledFormatters()
	{
	}

	public PersonalNameStyledFormatter Western { get; } = new(PersonalNameOrder.Western, PersonalNameStyle.Full);

	public PersonalNameStyledFormatter WesternCompact { get; } = new(
		PersonalNameOrder.Western,
		PersonalNameStyle.Compact
	);

	public PersonalNameStyledFormatter Eastern { get; } = new(PersonalNameOrder.Eastern, PersonalNameStyle.Full);

	public PersonalNameStyledFormatter EasternCompact { get; } = new(
		PersonalNameOrder.Eastern,
		PersonalNameStyle.Compact
	);
}