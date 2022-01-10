using Common.Core.Diagnostics;

namespace Common.Core;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class GuardedByAttribute : Attribute
{
	public GuardedByAttribute(string @lock)
	{
		Preconditions.RequiresNotNull(@lock, nameof(@lock));

		Lock = @lock;
	}

	public string Lock { get; }
}