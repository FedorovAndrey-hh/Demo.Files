using Common.Core.Diagnostics;

namespace Common.Core.Modifications;

public static class IdentityEqualityComparer
{
	public static IEqualityComparer<T> By<TIdentity, T>()
		where T : IIdentifiable<TIdentity>
		where TIdentity : IEquatable<TIdentity>
		=> IdentityTypedEqualityComparer<TIdentity, T>.Create();

	public static IEqualityComparer<IIdentifiable<TIdentity>> For<TIdentity>()
		where TIdentity : IEquatable<TIdentity>
		=> IdentityEqualityComparer<TIdentity>.Create();
}

public sealed class IdentityEqualityComparer<TIdentity> : IEqualityComparer<IIdentifiable<TIdentity>>
	where TIdentity : IEquatable<TIdentity>
{
	private static IdentityEqualityComparer<TIdentity>? _cache;

	public static IdentityEqualityComparer<TIdentity> Create()
		=> _cache ?? (_cache = new IdentityEqualityComparer<TIdentity>());

	public bool Equals(IIdentifiable<TIdentity>? x, IIdentifiable<TIdentity>? y)
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

	public int GetHashCode(IIdentifiable<TIdentity> obj)
	{
		Preconditions.RequiresNotNull(obj, nameof(obj));

		return obj.GetIdentityHashCode();
	}
}