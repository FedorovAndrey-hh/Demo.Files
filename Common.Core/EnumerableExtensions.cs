using System.Collections;
using Common.Core.Diagnostics;
using Common.Core.MathConcepts.GroupKind;

namespace Common.Core;

public static class EnumerableExtensions
{
	public static T Aggregate<T>(this IEnumerable<T> @this, T seed, IMagma<T> magma)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(seed, nameof(seed));
		Preconditions.RequiresNotNull(magma, nameof(magma));

		return @this.Aggregate(seed, magma.Combine);
	}

	public static T Aggregate<T>(this IEnumerable<T> @this, IMonoid<T> monoid)
		where T : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(monoid, nameof(monoid));

		return @this.Aggregate(monoid.Identity, monoid.Combine);
	}

	public static IEnumerable<object> OfType(this IEnumerable @this, Type type)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(type, nameof(type));

		return Result();

		IEnumerable<object> Result()
		{
			foreach (var item in @this)
			{
				var itemType = item.GetType();
				if (Eq.Value(itemType, type) || itemType.IsSubclassOf(type))
				{
					yield return item;
				}
			}
		}
	}

	public static IEnumerable<object> AsGeneric(this IEnumerable @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		if (@this is IEnumerable<object> generic)
		{
			return generic;
		}

		return Result();

		IEnumerable<object> Result()
		{
			foreach (var item in @this)
			{
				yield return item;
			}
		}
	}

	public static int GetSequenceHashCodeOfImplementation<TSource>(
		this IEnumerable<TSource> @this,
		IEqualityComparer<TSource> comparer)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(comparer, nameof(comparer));

		return @this switch
		{
			TSource[] array => array.GetSequenceHashCode(comparer),
			List<TSource> list => list.GetSequenceHashCode(comparer),
			IList<TSource> iList => iList.GetSequenceHashCode(comparer),
			_ => @this.GetSequenceHashCode(comparer)
		};
	}

	public static int GetSequenceHashCode<TSource>(
		this TSource[] @this,
		IEqualityComparer<TSource> comparer)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var result = new HashCode();

		for (var i = 0; i < @this.Length; i++)
		{
			result.Add(@this[i], comparer);
		}

		result.Add(@this.Length);

		return result.ToHashCode();
	}

	public static int GetSequenceHashCode<TSource>(
		this List<TSource> @this,
		IEqualityComparer<TSource> comparer)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var result = new HashCode();

		var count = @this.Count;

		for (var i = 0; i < count; i++)
		{
			result.Add(@this[i], comparer);
		}

		result.Add(count);

		return result.ToHashCode();
	}

	public static int GetSequenceHashCode<TSource>(
		this IList<TSource> @this,
		IEqualityComparer<TSource> comparer)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var result = new HashCode();

		var count = @this.Count;

		for (var i = 0; i < count; i++)
		{
			result.Add(@this[i], comparer);
		}

		result.Add(count);

		return result.ToHashCode();
	}

	public static int GetSequenceHashCode<TSource>(
		this IEnumerable<TSource> @this,
		IEqualityComparer<TSource> comparer)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		var result = new HashCode();

		var count = 0;

		foreach (var item in @this)
		{
			result.Add(item, comparer);

			count++;
		}

		result.Add(count);

		return result.ToHashCode();
	}

	public static bool None<TSource>(this IEnumerable<TSource> @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return !@this.Any();
	}

	public static bool None<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(predicate, nameof(predicate));

		return !@this.Any(predicate);
	}
}