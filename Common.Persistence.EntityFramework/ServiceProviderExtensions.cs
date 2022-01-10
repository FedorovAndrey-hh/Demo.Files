using Common.Core.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Persistence.EntityFramework;

public static class ServiceProviderExtensions
{
	public static async Task InitializeAllPersistenceAsync(this IServiceProvider provider)
	{
		Preconditions.RequiresNotNull(provider, nameof(provider));

		foreach (var initializer in provider.GetServices<IPersistenceInitializer>())
		{
			await initializer.InitializeAsync().ConfigureAwait(false);
		}
	}

	public static async Task WipeAllPersistenceAsync(this IServiceProvider provider)
	{
		Preconditions.RequiresNotNull(provider, nameof(provider));

		foreach (var wiper in provider.GetServices<IPersistenceWiper>())
		{
			await wiper.WiperAsync().ConfigureAwait(false);
		}
	}
}