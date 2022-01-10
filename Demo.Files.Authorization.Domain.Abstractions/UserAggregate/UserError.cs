namespace Demo.Files.Authorization.Domain.Abstractions.UserAggregate;

public enum UserError
{
	NotExists,
	InvalidHistory,
	Outdated,

	UsernameConflict,
	UsernameFormat,
	EmailConflict,
	Deactivated,

	ResourceConflict,
	ResourceAlreadyAcquired,
	ResourceNotRequested,
	DoesNotOwnResource,
	AlreadyActive,
	AlreadyDeactivated,

	PasswordEmpty,
	PasswordInvalidLength,
	PasswordUnsupportedCharacters,

	EmailEmpty,
	EmailInvalidFormat,
	EmailUnsupportedProvider,
	EmailUnsupportedCharacters,

	DisplayNameEmpty,
	DisplayNameTooLong,
	DisplayNameUnsupportedCharacters,
	DisplayNameInvalidFormat,
}