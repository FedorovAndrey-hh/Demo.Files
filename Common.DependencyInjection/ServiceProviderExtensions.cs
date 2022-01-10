using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Environments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.DependencyInjection;

public static class ServiceProviderExtensions
{
	public static TService CreateRequiredService<TService>(this IServiceProvider @this)
		where TService : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetRequiredService<IFactory<TService>>().Create();
	}

	public static TService CreateRequiredTaggedService<TService, TTag>(this IServiceProvider @this)
		where TService : notnull
		where TTag : notnull
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetRequiredService<ITaggedFactory<TService, TTag>>().Create();
	}

	public static T? GetOptions<T>(this IServiceProvider @this)
		where T : class
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetService<IOptions<T>>()?.Value;
	}

	public static T GetRequiredOptions<T>(this IServiceProvider @this)
		where T : class
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.GetRequiredService<IOptions<T>>().Value;
	}

	public static async Task WaitForEnvironmentSetupAsync(
		this IServiceProvider @this,
		CancellationToken cancellationToken = default)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		cancellationToken.ThrowIfCancellationRequested();
		await using (var scope = @this.CreateAsyncScope())
		{
			cancellationToken.ThrowIfCancellationRequested();
			var setups = scope.ServiceProvider.GetServices<IEnvironmentStateDetector>();

			cancellationToken.ThrowIfCancellationRequested();
			foreach (var setup in setups)
			{
				await setup.WaitReadyAsync(cancellationToken).ConfigureAwait(false);
			}
		}
	}
}