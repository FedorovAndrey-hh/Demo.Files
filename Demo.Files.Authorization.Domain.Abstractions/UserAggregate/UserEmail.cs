using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Emails;
using Common.Core.Text;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed record UserEmail
{
	public static IImmutableList<string> SupportedProviders { get; }
		= ImmutableArray.Create("gmail.com", "yandex.ru");

	public static UserEmail Create(string email)
	{
		if (!Parser.Parse(email, out var result, out var error))
		{
			throw new UserException(error);
		}

		return result;
	}

	private UserEmail(Email value)
	{
		Preconditions.RequiresNotNull(value, nameof(value));

		Value = value;
	}

	public Email Value { get; }

	public static IParser<UserEmail, UserError> Parser => ValueParser.Create();

	private sealed class ValueParser : IParser<UserEmail, UserError>
	{
		public static ValueParser? _cache;
		public static ValueParser Create() => _cache ?? (_cache = new ValueParser());

		private ValueParser()
		{
		}

		public bool Validate(string? data, out UserError error)
		{
			if (data.IsNullOrEmpty())
			{
				error = UserError.EmailEmpty;
				return false;
			}

			if (!Email.Parser.Parse(data.ToLowerInvariant(), out var value, out _))
			{
				error = UserError.EmailInvalidFormat;
				return false;
			}

			if (!SupportedProviders.Contains(value.Domain))
			{
				error = UserError.EmailUnsupportedProvider;
				return false;
			}

			if (!value.LocalPart.All(e => e.IsLetterOrDigit() || e.IsPunctuation()))
			{
				error = UserError.EmailUnsupportedCharacters;
				return false;
			}

			error = default;
			return true;
		}

		public bool Parse(string? source, [NotNullWhen(true)] out UserEmail? result, out UserError error)
		{
			if (source.IsNullOrEmpty())
			{
				result = null;
				error = UserError.EmailEmpty;
				return false;
			}

			if (!Email.Parser.Parse(source.ToLowerInvariant(), out var value, out _))
			{
				result = null;
				error = UserError.EmailInvalidFormat;
				return false;
			}

			if (!SupportedProviders.Contains(value.Domain))
			{
				result = null;
				error = UserError.EmailUnsupportedProvider;
				return false;
			}

			if (!value.LocalPart.All(e => e.IsLetterOrDigit() || e.IsPunctuation()))
			{
				result = null;
				error = UserError.EmailUnsupportedCharacters;
				return false;
			}

			result = new UserEmail(value);
			error = default;
			return true;
		}
	}
}