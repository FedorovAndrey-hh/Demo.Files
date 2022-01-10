using System.Diagnostics.CodeAnalysis;
using Common.Core.Text;

namespace Common.Core.Emails;

public sealed class EmailParser : IParser<Email, EmailParseError>
{
	private static EmailParser? _cache = new();
	public static EmailParser Create() => _cache ?? (_cache = new EmailParser());

	private EmailParser()
	{
	}

	public bool Validate(string? data, out EmailParseError error)
	{
		if (data.IsNullOrEmpty())
		{
			error = EmailParseError.Empty;
			return false;
		}

		var parts = data.Split(Email.DomainSeparator, 3, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length != 2)
		{
			error = EmailParseError.InvalidSyntax;
			return false;
		}

		error = default;
		return true;
	}

	public bool Parse(string? source, [NotNullWhen(true)] out Email? result, out EmailParseError error)
	{
		if (source.IsNullOrEmpty())
		{
			result = null;
			error = EmailParseError.Empty;
			return false;
		}

		var parts = source.Split(Email.DomainSeparator, 3, StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length != 2)
		{
			result = null;
			error = EmailParseError.InvalidSyntax;
			return false;
		}

		result = Email.Create(parts[0], parts[1]);
		error = default;
		return true;
	}
}