using System.Collections;
using Common.Core.Diagnostics;

namespace Common.Core;

public static class Option
{
	public static Option<T> None<T>()
		where T : notnull
		=> Option<T>.None.Create();

	public static Option<T> Some<T>(T value)
		where T : notnull
	{
		Preconditions.RequiresNotNull(value, nameof(value));

		return new Option<T>.Some(value);
	}

	public static Option<T> OfNullable<T>(T? value)
		where T : notnull
		=> value is null ? None<T>() : Some(value);

	public static Option<T> Try<T>(Func<T> action)
		where T : notnull
	{
		Preconditions.RequiresNotNull(action, nameof(action));

		try
		{
			return Some(Contracts.NotNullReturnFrom(action(), nameof(action)));
		}
		catch
		{
			return None<T>();
		}
	}
}

public abstract class Option<T>
	: IReadOnlyList<T>,
	  IEquatable<Option<T>>
	where T : notnull
{
	public abstract bool IsEmpty { get; }

	public abstract TResult Fold<TResult>(Func<T, TResult> ifSome, Func<TResult> ifEmpty)
		where TResult : notnull;

	public abstract T Or(T alternative);

	public abstract T OrGet(Func<T> alternative);

	public abstract T? OrDefault();

	public abstract Option<T> OrElse(Option<T> alternative);
	public abstract Option<T> OrElseGet(Func<Option<T>> alternative);

	public abstract Option<TResult> Map<TResult>(Func<T, TResult> mapper)
		where TResult : notnull;

	public abstract Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapper)
		where TResult : notnull;

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public abstract IEnumerator<T> GetEnumerator();
	public abstract int Count { get; }
	public abstract T this[int index] { get; }
	public abstract bool Equals(Option<T>? other);

	internal sealed class None : Option<T>
	{
		private static None? _cache;
		internal static None Create() => _cache ?? (_cache = new None());

		public override bool IsEmpty { get; } = true;

		public override TResult Fold<TResult>(Func<T, TResult> ifSome, Func<TResult> ifEmpty)
		{
			Preconditions.RequiresNotNull(ifSome, nameof(ifSome));
			Preconditions.RequiresNotNull(ifEmpty, nameof(ifEmpty));

			return Contracts.NotNullReturnFrom(ifEmpty(), nameof(ifEmpty));
		}

		public override T Or(T alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return alternative;
		}

		public override T OrGet(Func<T> alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return Contracts.NotNullReturnFrom(alternative(), nameof(alternative));
		}

		public override T? OrDefault() => default;

		public override Option<T> OrElse(Option<T> alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return alternative;
		}

		public override Option<T> OrElseGet(Func<Option<T>> alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return Contracts.NotNullReturnFrom(alternative(), nameof(alternative));
		}

		public override Option<TResult> Map<TResult>(Func<T, TResult> mapper)
		{
			Preconditions.RequiresNotNull(mapper, nameof(mapper));

			return new Option<TResult>.None();
		}

		public override Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapper)
		{
			Preconditions.RequiresNotNull(mapper, nameof(mapper));

			return new Option<TResult>.None();
		}

		public override int Count => 0;

		public override T this[int index] => throw new IndexOutOfRangeException();

		public override IEnumerator<T> GetEnumerator()
		{
			yield break;
		}

		public override int GetHashCode() => HashCode.Combine(GetType());

		public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

		public override bool Equals(Option<T>? other) => other is None;
	}

	internal sealed class Some : Option<T>
	{
		internal Some(T value)
		{
			Preconditions.RequiresNotNull(value, nameof(value));

			_value = value;
		}

		private readonly T _value;

		public override bool IsEmpty { get; } = false;

		public override TResult Fold<TResult>(Func<T, TResult> ifSome, Func<TResult> ifEmpty)
		{
			Preconditions.RequiresNotNull(ifSome, nameof(ifSome));
			Preconditions.RequiresNotNull(ifEmpty, nameof(ifEmpty));

			return Contracts.NotNullReturnFrom(ifSome(_value), nameof(ifSome));
		}

		public override T Or(T alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return _value;
		}

		public override T OrGet(Func<T> alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return _value;
		}

		public override T OrDefault() => _value;

		public override Option<T> OrElse(Option<T> alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return this;
		}

		public override Option<T> OrElseGet(Func<Option<T>> alternative)
		{
			Preconditions.RequiresNotNull(alternative, nameof(alternative));

			return this;
		}

		public override Option<TResult> Map<TResult>(Func<T, TResult> mapper)
		{
			Preconditions.RequiresNotNull(mapper, nameof(mapper));

			return new Option<TResult>.Some(Contracts.NotNullReturnFrom(mapper(_value), nameof(mapper)));
		}

		public override Option<TResult> FlatMap<TResult>(Func<T, Option<TResult>> mapper)
		{
			Preconditions.RequiresNotNull(mapper, nameof(mapper));

			return Contracts.NotNullReturnFrom(mapper(_value), nameof(mapper));
		}

		public override int Count => 1;

		public override T this[int index] => index == 0 ? _value : throw new IndexOutOfRangeException();

		public override IEnumerator<T> GetEnumerator()
		{
			yield return _value;
		}

		public override int GetHashCode() => HashCode.Combine(GetType(), _value);

		public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

		public override bool Equals(Option<T>? other) => other is Some otherSome && Eq.Value(_value, otherSome._value);
	}

	private Option()
	{
	}
}