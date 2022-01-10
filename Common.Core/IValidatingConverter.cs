using System.Diagnostics.CodeAnalysis;
using Common.Core.Data;

namespace Common.Core;

public interface IValidatingConverter<in TSource, TTarget, TError>
	: IConverter<TSource, TTarget>,
	  IValidator<TSource, TError>
	where TError : notnull
{
	public bool TryConvert(
		string? source,
		[NotNullWhen(true)] out TTarget? result,
		[NotNullWhen(false)] out TError? error);
}