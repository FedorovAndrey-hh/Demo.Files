using Common.Core.Diagnostics;

namespace Common.Core.Modifications;

public static class IdentifiableExtensions
{
	public static bool SameIdentity<TIdentity>(this IIdentifiable<TIdentity> @this, IIdentifiable<TIdentity>? other)
		where TIdentity : IEquatable<TIdentity>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return other is not null && @this.IdentityEquals(other.Id);
	}

	public static bool IdentityEquals<TIdentity>(this IIdentifiable<TIdentity> @this, TIdentity? other)
		where TIdentity : IEquatable<TIdentity>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return other is not null && Eq.ValueSafe(@this.Id, other);
	}
	
	public static int GetIdentityHashCode<TIdentity>(this IIdentifiable<TIdentity> @this)
		where TIdentity : IEquatable<TIdentity>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Id.GetHashCode();
	}
}