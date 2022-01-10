using Common.Core;
using Common.Core.Diagnostics;
using Common.Core.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Persistence.EntityFramework;

public static class ServiceCollectionExtensions
{
	public static void AddDatabaseEnvironmentStateDetector<TContext>(
		this IServiceCollection @this,
		TimeSpan checkPeriod)
		where TContext : DbContext
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.AddScoped<IEnvironmentStateDetector>(
			services => new EntityFrameworkEnvironmentStateDetector(
				MathExtensions.Max(TimeSpan.FromSeconds(1), checkPeriod),
				services.GetRequiredService<TContext>()
			)
		);
	}
}