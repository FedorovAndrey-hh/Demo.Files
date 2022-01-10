namespace Demo.Files.FilesManagement.Domain.Abstractions.StorageAggregate;

public enum StorageError
{
	NotExists,
	Outdated,

	InvalidHistory,

	ExceededLimitations,

	LimitationsTotalSpaceTooLarge,
	LimitationsTotalFileCountTooLarge,
	LimitationsTotalSingleFileSizeTooLarge,

	DirectoryEmptyName,
	DirectoryNameWithUnsupportedCharacters,
	DirectoryNameTooLarge,
	DirectoryNameConflict,
	DirectoryNotExists,

	FileEmptyName,
	FileNameWithUnsupportedCharacters,
	FileNameTooLarge,
	FileNameConflict,
	FileNotExists,
	FileIllegalMove
}