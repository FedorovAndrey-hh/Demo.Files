using System.Diagnostics.CodeAnalysis;

namespace Common.Core.Data;

public interface IValidator<in TData, TError>
	where TError : notnull
{
	public bool Validate(TData? data, [NotNullWhen(false)] out TError? error);
}