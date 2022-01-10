using System.Diagnostics.CodeAnalysis;
using Common.Core.Diagnostics;

namespace Common.Core;

public static class StringExtensions
{
	public static bool IsNullOrEmpty([NotNullWhen(false)] this string? @this) => string.IsNullOrEmpty(@this);

	public static string? NullIfEmpty(this string? @this) => @this.IsNullOrEmpty() ? null : @this;

	public static string OrEmpty(this string? @this) => @this ?? string.Empty;

	public static string AfterFirst(
		this string @this,
		string delimiter,
		StringComparison comparisonType = StringComparison.Ordinal)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(delimiter, nameof(delimiter));

		var index = @this.IndexOf(delimiter, comparisonType);

		return index < 0 ? string.Empty : @this[(index + delimiter.Length) ..];
	}

	public static string AfterFirst(
		this string @this,
		char delimiter,
		StringComparison comparisonType = StringComparison.Ordinal)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var index = @this.IndexOf(delimiter, comparisonType);

		return index < 0 ? string.Empty : @this[(index + 1) ..];
	}

	public static string AfterLast(
		this string @this,
		string delimiter,
		StringComparison comparisonType = StringComparison.Ordinal)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(delimiter, nameof(delimiter));

		var index = @this.LastIndexOf(delimiter, comparisonType);

		return index < 0 ? string.Empty : @this[(index + delimiter.Length) ..];
	}

	public static string AfterLast(
		this string @this,
		char delimiter,
		StringComparison comparisonType = StringComparison.Ordinal)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var index = @this.LastIndexOf(delimiter.ToString(), comparisonType);

		return index < 0 ? string.Empty : @this[(index + 1) ..];
	}

	public static string BeforeFirst(
		this string @this,
		string delimiter,
		StringComparison comparisonType = StringComparison.Ordinal)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(delimiter, nameof(delimiter));

		var index = @this.IndexOf(delimiter, comparisonType);

		return index < 0 ? string.Empty : @this[..index];
	}

	public static string BeforeFirst(
		this string @this,
		char delimiter,
		StringComparison comparisonType = StringComparison.Ordinal)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var index = @this.IndexOf(delimiter, comparisonType);

		return index < 0 ? string.Empty : @this[..index];
	}
}