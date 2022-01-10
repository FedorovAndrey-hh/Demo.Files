using System.Resources;
using Common.Core.Diagnostics;

namespace Common.Core;

public static class ResourceManagerUtility
{
	public static ResourceManager ToResourceManager(this Type @this)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return new ResourceManager(@this.FullName!, @this.Assembly);
	}
}