using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using Common.Core.Intervals;
using JetBrains.Annotations;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed record Password : StringWrapper<Password>
{
	public static IInterval<uint> LengthInterval { get; } = ComparerInterval.Inclusive(6u, 25u);

	[ContractAnnotation("value: null => nothing")]
	public static Password Create(string value)
	{
		if (!Validator.Validate(value, out var error))
		{
			throw new UserException(error);
		}

		return new Password(value);
	}

	private Password(string value)
		: base(Preconditions.RequiresNotNull(value, nameof(value)), StringComparison.Ordinal)
	{
	}

	public string AsString() => Value;

	public static IValidator<string, UserError> Validator => ValueValidator.Create();

	private sealed class ValueValidator : IValidator<string, UserError>
	{
		private static ValueValidator? _cache;
		public static ValueValidator Create() => _cache ?? (_cache = new ValueValidator());

		private ValueValidator()
		{
		}

		public bool Validate(string? data, out UserError error)
		{
			if (data.IsNullOrEmpty())
			{
				error = UserError.PasswordEmpty;
				return false;
			}

			if (!LengthInterval.Contains((uint)data.Length))
			{
				error = UserError.PasswordInvalidLength;
				return false;
			}

			if (!data.All(e => e.IsLetterOrDigit() || e.IsPunctuation()))
			{
				error = UserError.PasswordUnsupportedCharacters;
				return false;
			}

			error = default;
			return true;
		}
	}
}