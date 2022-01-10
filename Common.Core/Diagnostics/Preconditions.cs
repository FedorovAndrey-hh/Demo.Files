using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Common.Core.Intervals;
using JetBrains.Annotations;

namespace Common.Core.Diagnostics;

public static class Preconditions
{
	[ContractAnnotation("condition: false => nothing")]
	public static void Requires([DoesNotReturnIf(false)] bool condition, string? description)
	{
		if (!condition)
		{
			throw new PreconditionException(description);
		}
	}

	[ContractAnnotation("target: null => nothing; target: notnull => notnull")]
	[return: System.Diagnostics.CodeAnalysis.NotNull, NotNullIfNotNull("target")]
	public static T RequiresNotNull<T>([NoEnumeration] T? target, string? name)
	{
		Requires(target is not null, name is null ? "Value must not be null." : $"`{name}` must not be null.");

		return target;
	}

	[ContractAnnotation("target: null => nothing; target: notnull => notnull")]
	public static T RequiresNotNullItems<T>(T target, string? name)
		where T : IEnumerable
	{
		RequiresNotNull(target, nameof(target));

		foreach (var item in target)
		{
			Requires(
				item is not null,
				name is null ? "Collection must not contain null." : $"`{name}` must not contain null."
			);
		}

		return target;
	}

	[ContractAnnotation("target: null => nothing; target: notnull => notnull")]
	[return: System.Diagnostics.CodeAnalysis.NotNull, NotNullIfNotNull("target")]
	public static T RequiresNotDefault<T>([NoEnumeration] T? target, string? name)
	{
		var @default = default(T);
		Requires(
			(@default is null && target is not null) || (@default is not null && !@default.Equals(target)),
			name is null ? "Value must not be null." : $"`{name}` must not be null."
		);

		return target!;
	}

	public static T RequiresPositive<T>(T target, string? name)
		where T : struct, IComparable<T>
	{
		Requires(
			target.CompareTo(default) > 0,
			name is null
				? $"Value must be positive (actual `{target}`)."
				: $"`{name}` must be positive (actual `{target}`)."
		);

		return target;
	}

	public static T RequiresNonNegative<T>(T target, string? name)
		where T : struct, IComparable<T>
	{
		Requires(
			target.CompareTo(default) >= 0,
			name is null
				? $"Value must be non-negative (actual `{target}`)."
				: $"`{name}` must be non-negative (actual `{target}`)."
		);

		return target;
	}

	public static T RequiresNegative<T>(T target, string? name)
		where T : struct, IComparable<T>
	{
		Requires(
			target.CompareTo(default) < 0,
			name is null
				? $"Value must be negative (actual `{target}`)."
				: $"`{name}` must be negative (actual `{target}`)."
		);

		return target;
	}

	public static T RequiresNonPositive<T>(T target, string? name)
		where T : struct, IComparable<T>
	{
		Requires(
			target.CompareTo(default) <= 0,
			name is null
				? $"Value must be non-positive (actual `{target}`)."
				: $"`{name}` must be non-positive (actual `{target}`)."
		);

		return target;
	}

	public static T RequiresInRange<T>(T target, IInterval<T> interval, string? name)
		where T : notnull
	{
		RequiresNotNull(target, nameof(target));
		RequiresNotNull(interval, nameof(interval));

		Requires(
			interval.Contains(target),
			name is null
				? $"Value must be in range {interval.FormatInMathNotation()} (actual `{target}`)."
				: $"`{name}` must be in range {interval.FormatInMathNotation()} (actual `{target}`)."
		);

		return target;
	}

	[ContractAnnotation("target: null => null")]
	[return: NotNullIfNotNull("target")]
	public static string? RequiresNotEmpty(string? target, string? name)
	{
		Requires(
			target is null || !target.IsNullOrEmpty(),
			name is null ? "Value must not be an empty string." : $"`{name}` must not be an empty string."
		);

		return target;
	}

	public static void RequiresIdenticalType<TExpected>(object target, string? name)
	{
		RequiresNotNull(target, nameof(target));

		Requires(
			!target.GetType().Equals<TExpected>(),
			name is null
				? $"Value must be of type `{typeof(TExpected).FullName}`."
				: $"`{name}` must be of type `{typeof(TExpected).FullName}`."
		);
	}
}