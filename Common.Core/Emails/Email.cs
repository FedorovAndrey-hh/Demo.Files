using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Common.Core.Emails;

public class Email
	: IFormattable,
	  IExternalFormattable<Email>,
	  IEquatable<Email>
{
	public static char DomainSeparator => '@';

	public static EmailParser Parser => EmailParser.Create();
	public static EmailFormatter Formatter => EmailFormatter.Create();

	public static Email Create(string localPart, string domain)
	{
		Preconditions.RequiresNotNull(localPart, nameof(localPart));
		Preconditions.RequiresNotEmpty(localPart, nameof(localPart));
		Preconditions.Requires(
			!localPart.Contains(DomainSeparator),
			$"{nameof(localPart)} can't contain `{DomainSeparator}`."
		);
		Preconditions.RequiresNotNull(domain, nameof(domain));
		Preconditions.RequiresNotEmpty(domain, nameof(domain));
		Preconditions.Requires(
			!domain.Contains(DomainSeparator),
			$"{nameof(localPart)} can't contain `{DomainSeparator}`."
		);

		return new Email(localPart, domain);
	}

	private Email(string localPart, string domain)
	{
		LocalPart = localPart;
		Domain = domain;
	}

	public string LocalPart { get; }

	public string Domain { get; }

	public string ToString(IExternalFormatter<Email> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public string ToString(string? format, IFormatProvider? formatProvider)
		=> this.HandleCustomFormatters(format, formatProvider) ?? ToString();

	public override string ToString() => LocalPart + DomainSeparator + Domain;

	public static bool operator ==(Email? lhs, Email? rhs) => Eq.ValueSafe(lhs, rhs);

	public static bool operator !=(Email? lhs, Email? rhs) => !Eq.ValueSafe(lhs, rhs);

	public bool Equals(Email? other)
		=> ReferenceEquals(other, this)
		   || (other is not null
		       && string.Equals(LocalPart, other.LocalPart, StringComparison.Ordinal)
		       && string.Equals(Domain, other.Domain, StringComparison.Ordinal));

	public override bool Equals(object? obj) => ReferenceEquals(obj, this) || (obj is Email email && Equals(email));

	public override int GetHashCode()
		=> HashCode.Combine(
			LocalPart.GetHashCode(StringComparison.Ordinal),
			Domain.GetHashCode(StringComparison.Ordinal)
		);
}