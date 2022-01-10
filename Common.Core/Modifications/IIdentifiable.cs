namespace Common.Core.Modifications;

public interface IIdentifiable<TIdentity>
	where TIdentity : IEquatable<TIdentity>
{
	public TIdentity Id { get; }
}