using System.Diagnostics.CodeAnalysis;
using Common.Core.Data;

namespace Common.Core.Text;

public interface IParser<TResult, TError> : IValidator<string, TError>
	where TResult : notnull
	where TError : notnull
{
	public bool Parse(
		string? source,
		[NotNullWhen(true)] out TResult? result,
		[NotNullWhen(false)] out TError? error);
}