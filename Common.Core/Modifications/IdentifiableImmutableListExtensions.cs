using System.Collections.Immutable;
using Common.Core.Diagnostics;

namespace Common.Core.Modifications;

public static class IdentifiableImmutableListExtensions
{
	public static IImmutableList<T> RemoveById<T, TIdentity>(this IImmutableList<T> @this, T value)
		where T : IIdentifiable<TIdentity>
		where TIdentity : IEquatable<TIdentity>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Remove(value, IdentityEqualityComparer.By<TIdentity, T>());
	}

	public static IImmutableList<T> ReplaceById<T, TIdentity>(this IImmutableList<T> @this, T newValue)
		where T : IIdentifiable<TIdentity>
		where TIdentity : IEquatable<TIdentity>
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.Replace(newValue, newValue, IdentityEqualityComparer.By<TIdentity, T>());
	}
}