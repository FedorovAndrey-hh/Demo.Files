using System.Diagnostics.CodeAnalysis;
using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Text;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed class Username
	: IEquatable<Username>,
	  IExternalFormattable<Username>
{
	public const char QuantifierDivider = '#';

	public static Username Create(UserDisplayName displayName, ushort quantifier)
	{
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		return new Username(displayName, quantifier);
	}

	private Username(UserDisplayName displayName, ushort quantifier)
	{
		Preconditions.RequiresNotNull(displayName, nameof(displayName));

		DisplayName = displayName;
		Quantifier = quantifier;
	}

	public UserDisplayName DisplayName { get; }
	public ushort Quantifier { get; }

	public bool Equals(Username? other)
		=> other is not null
		   && Eq.ValueSafe(DisplayName, other.DisplayName)
		   && Eq.StructSafe(Quantifier, other.Quantifier);

	public override bool Equals(object? obj) => obj is Username other && Equals(other);

	public override int GetHashCode() => HashCode.Combine(DisplayName, Quantifier);

	public string ToString(IExternalFormatter<Username> formatter)
	{
		Preconditions.RequiresNotNull(formatter, nameof(formatter));

		return formatter.Format(this);
	}

	public override string ToString() => ToString(Formatter);

	public static IParser<Username, UserError> Parser => ExternalFormatter.Create();
	public static IExternalFormatter<Username> Formatter => ExternalFormatter.Create();

	private sealed class ExternalFormatter
		: IExternalFormatter<Username>,
		  IParser<Username, UserError>
	{
		private static ExternalFormatter? _cache;

		public static ExternalFormatter Create() => _cache ?? (_cache = new ExternalFormatter());

		private ExternalFormatter()
		{
		}

		public string Format(Username data)
		{
			Preconditions.RequiresNotNull(data, nameof(data));

			return $"{data.DisplayName.AsString()}{QuantifierDivider}{data.Quantifier:0000}";
		}

		public bool Parse(string? source, [NotNullWhen(true)] out Username? result, out UserError error)
		{
			if (source is null)
			{
				result = null;
				error = UserError.UsernameFormat;
				return false;
			}

			var dividerIndex = source.LastIndexOf(QuantifierDivider);
			if (dividerIndex <= 0)
			{
				result = null;
				error = UserError.UsernameFormat;
				return false;
			}

			var rawDisplayName = source[..dividerIndex];
			var rawQuantifier = source[(dividerIndex + 1)..];

			if (!UserDisplayName.Validator.Validate(rawDisplayName, out error))
			{
				result = null;
				return false;
			}

			if (!ushort.TryParse(rawQuantifier, out var quantifier))
			{
				result = null;
				error = UserError.UsernameFormat;
				return false;
			}

			result = Username.Create(UserDisplayName.Create(rawDisplayName), quantifier);
			error = default;
			return true;
		}

		public bool Validate(string? data, out UserError error)
		{
			if (data is null)
			{
				error = UserError.UsernameFormat;
				return false;
			}

			var dividerIndex = data.LastIndexOf(QuantifierDivider);
			if (dividerIndex <= 0)
			{
				error = UserError.UsernameFormat;
				return false;
			}

			var rawDisplayName = data[..dividerIndex];
			var rawQuantifier = data[(dividerIndex + 1)..];

			if (!UserDisplayName.Validator.Validate(rawDisplayName, out error))
			{
				return false;
			}

			if (!ushort.TryParse(rawQuantifier, out _))
			{
				error = UserError.UsernameFormat;
				return false;
			}

			error = default;
			return true;
		}
	}
}