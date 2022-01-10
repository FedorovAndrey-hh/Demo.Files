using Common.Core;
using Common.Core.Diagnostics;
using JetBrains.Annotations;

namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public sealed record FileName : StringWrapper<FileName>
{
	public static FileName Create(string value)
	{
		_Validate(value);

		return new FileName(value);
	}

	[ContractAnnotation("value: null => nothing")]
	private static void _Validate(string? value)
	{
		if (value.IsNullOrEmpty())
		{
			throw new StorageException(StorageError.FileEmptyName);
		}

		if (value.Length >= 255)
		{
			throw new StorageException(StorageError.FileNameTooLarge);
		}

		if (!value.All(e => e.IsLetterOrDigit() || e == '_'))
		{
			throw new StorageException(StorageError.FileNameWithUnsupportedCharacters);
		}
	}

	private FileName(string value)
		: base(Preconditions.RequiresNotNull(value, nameof(value)), StringComparison.Ordinal)
	{
	}

	public string AsString() => Value;
}