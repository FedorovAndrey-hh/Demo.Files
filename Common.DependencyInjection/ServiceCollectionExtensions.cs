using Common.Core.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Common.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static void AddConfiguration(
		this IServiceCollection @this,
		Action<IConfigurationBuilder> build,
		Action<IConfiguration> config)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(build, nameof(build));
		Preconditions.RequiresNotNull(config, nameof(config));

		var builder = new ConfigurationBuilder();
		build(builder);

		var configuration = builder.Build();

		config(configuration);

		@this.AddSingleton<IConfiguration>(configuration);
	}

	public static void Add<T>(
		this IServiceCollection @this,
		ServiceImplementation<T> implementation,
		ServiceLifetime lifetime)
		where T : class
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		@this.Add(implementation.ToServiceDescriptor(lifetime));
	}

	public static void AddSingleton<T>(this IServiceCollection @this, ServiceImplementation<T> implementation)
		where T : class
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		@this.Add(implementation, ServiceLifetime.Singleton);
	}

	public static void AddScoped<T>(this IServiceCollection @this, ServiceImplementation<T> implementation)
		where T : class
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		@this.Add(implementation, ServiceLifetime.Scoped);
	}

	public static void AddTransient<T>(this IServiceCollection @this, ServiceImplementation<T> implementation)
		where T : class
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		@this.Add(implementation, ServiceLifetime.Transient);
	}

	public static void Install<T>(this IServiceCollection @this)
		where T : IServicesPlugin, new()
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.Install(new T());
	}

	public static void Install(this IServiceCollection @this, IServicesPlugin plugin)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(plugin, nameof(plugin));

		plugin.InstallTo(@this);
	}

	public static void InstallAll(this IServiceCollection @this, IEnumerable<IServicesPlugin> plugins)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(plugins, nameof(plugins));

		foreach (var plugin in plugins)
		{
			plugin.InstallTo(@this);
		}
	}

	public static void TryAddSingletonBinding<TService, TImplementation>(this IServiceCollection @this)
		where TService : class
		where TImplementation : class, TService
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.TryAddSingleton<TService>(services => services.GetRequiredService<TImplementation>());
	}

	public static void TryAddScopedBinding<TService, TImplementation>(this IServiceCollection @this)
		where TService : class
		where TImplementation : class, TService
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.TryAddScoped<TService>(services => services.GetRequiredService<TImplementation>());
	}

	public static void TryAddTransientBinding<TService, TImplementation>(this IServiceCollection @this)
		where TService : class
		where TImplementation : class, TService
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		@this.TryAddTransient<TService>(services => services.GetRequiredService<TImplementation>());
	}

	public static IServiceCollection AddSingletonBinding<TService, TImplementation>(this IServiceCollection @this)
		where TService : class
		where TImplementation : class, TService
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.AddSingleton<TService>(services => services.GetRequiredService<TImplementation>());
	}

	public static IServiceCollection AddSingletonBinding(
		this IServiceCollection @this,
		Type serviceType,
		Type implementationType)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(serviceType, nameof(serviceType));
		Preconditions.RequiresNotNull(implementationType, nameof(implementationType));

		return @this.AddSingleton(serviceType, services => services.GetRequiredService(implementationType));
	}

	public static IServiceCollection AddScopedBinding<TService, TImplementation>(this IServiceCollection @this)
		where TService : class
		where TImplementation : class, TService
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.AddScoped<TService>(services => services.GetRequiredService<TImplementation>());
	}

	public static IServiceCollection AddScopedBinding(
		this IServiceCollection @this,
		Type serviceType,
		Type implementationType)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(serviceType, nameof(serviceType));
		Preconditions.RequiresNotNull(implementationType, nameof(implementationType));

		return @this.AddScoped(serviceType, services => services.GetRequiredService(implementationType));
	}

	public static IServiceCollection AddTransientBinding<TService, TImplementation>(this IServiceCollection @this)
		where TService : class
		where TImplementation : class, TService
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));

		return @this.AddTransient<TService>(services => services.GetRequiredService<TImplementation>());
	}

	public static IServiceCollection AddTransientBinding(
		this IServiceCollection @this,
		Type serviceType,
		Type implementationType)
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(serviceType, nameof(serviceType));
		Preconditions.RequiresNotNull(implementationType, nameof(implementationType));

		return @this.AddTransient(serviceType, services => services.GetRequiredService(implementationType));
	}
}