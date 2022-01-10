namespace Common.Core.Modifications;

public interface IVersionable<TVersion>
	where TVersion : IVersion<TVersion>
{
	public TVersion Version { get; }
}