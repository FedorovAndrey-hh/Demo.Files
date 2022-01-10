using Common.Core.Diagnostics;

namespace Common.Web;

public static class ApplicationBuilderExtensions
{
	public static void UsePlugin(this IApplicationBuilder @this, IApplicationPlugin plugin)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(plugin, nameof(plugin));

		plugin.UseIn(@this);
	}
}