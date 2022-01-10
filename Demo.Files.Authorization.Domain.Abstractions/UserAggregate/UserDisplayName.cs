using Common.Core;
using Common.Core.Data;
using Common.Core.Diagnostics;
using JetBrains.Annotations;

namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public sealed record UserDisplayName : StringWrapper<UserDisplayName>
{
	public static UserDisplayName Create(string value)
	{
		_Validate(value);

		return new UserDisplayName(value);
	}

	public static IValidator<string, UserError> Validator => ValueValidator.Create();

	private const uint _maxLength = 24;
	private static readonly string _validSpecialCharacters = @"_-+=/\{}()<>[]!?$";

	[ContractAnnotation("value: null => nothing")]
	private static void _Validate(string value)
	{
		if (!Validator.Validate(value, out var error))
		{
			throw new UserException(error);
		}
	}

	private UserDisplayName(string value)
		: base(Preconditions.RequiresNotNull(value, nameof(value)), StringComparison.Ordinal)
	{
	}

	public string AsString() => Value;

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
				error = UserError.DisplayNameEmpty;
				return false;
			}

			if (data.Length > _maxLength)
			{
				error = UserError.DisplayNameTooLong;
				return false;
			}

			if (!data.All(e => e.IsLetterOrDigit() || _validSpecialCharacters.Contains(e)))
			{
				error = UserError.DisplayNameUnsupportedCharacters;
				return false;
			}

			if (!data[0].IsLetterOrDigit())
			{
				error = UserError.DisplayNameInvalidFormat;
				return false;
			}

			error = default;
			return true;
		}
	}
}