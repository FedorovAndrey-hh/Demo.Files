using Common.Core;
using Common.Core.Diagnostics;
using JetBrains.Annotations;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed record DirectoryName : StringWrapper<DirectoryName>
{
	public static DirectoryName Create(string value)
	{
		_Validate(value);

		return new DirectoryName(value);
	}

	private static readonly string _validSpecialCharacters = @"_-/\{}()";

	[ContractAnnotation("value: null => nothing")]
	private static void _Validate(string? value)
	{
		if (value.IsNullOrEmpty())
		{
			throw new StorageException(StorageError.DirectoryEmptyName);
		}

		if (value.Length >= 255)
		{
			throw new StorageException(StorageError.DirectoryNameTooLarge);
		}

		if (!value.All(e => e.IsLetterOrDigit() || _validSpecialCharacters.Contains(e)))
		{
			throw new StorageException(StorageError.DirectoryNameWithUnsupportedCharacters);
		}
	}

	private DirectoryName(string value)
		: base(Preconditions.RequiresNotNull(value, nameof(value)), StringComparison.Ordinal)
	{
	}

	public string AsString() => Value;
}