using Common.Core.Diagnostics;

namespace Common.Core.Modifications;

public sealed class IdentityTypedEqualityComparer<TIdentity, T> : IEqualityComparer<T>
	where T : IIdentifiable<TIdentity>
	where TIdentity : IEquatable<TIdentity>
{
	private static IdentityTypedEqualityComparer<TIdentity, T>? _cache;
	public static IdentityTypedEqualityComparer<TIdentity, T> Create()
		=> _cache ?? (_cache = new IdentityTypedEqualityComparer<TIdentity, T>());

	public bool Equals(T? x, T? y)
	{
		if (ReferenceEquals(x, y))
		{
			return true;
		}

		if (x is null || y is null)
		{
			return false;
		}

		return x.SameIdentity(y);
	}

	public int GetHashCode(T obj)
	{
		Preconditions.RequiresNotNull(obj, nameof(obj));

		return obj.GetIdentityHashCode();
	}
}