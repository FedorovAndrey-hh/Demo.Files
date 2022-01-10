using Common.Core.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection;

public static class ServiceImplementation
{
	public static ServiceImplementation<TService> Instance<TService>(TService instance)
		where TService : class
	{
		Preconditions.RequiresNotNull(instance, nameof(instance));

		return new ServiceImplementation<TService>(implementationInstance: instance);
	}

	public static ServiceImplementation<TService> Type<TService, TImplementation>()
		where TService : class
		where TImplementation : TService
		=> new(implementationType: typeof(TImplementation));

	public static ServiceImplementation<TService> Binding<TService, TImplementation>()
		where TService : class
		where TImplementation : TService
		=> new(bindingType: typeof(TImplementation));

	public static ServiceImplementation<TService> Factory<TService>(Func<IServiceProvider, TService> factory)
		where TService : class
	{
		Preconditions.RequiresNotNull(factory, nameof(factory));

		return new ServiceImplementation<TService>(implementationFactory: factory);
	}
}

public sealed class ServiceImplementation<TService>
	where TService : class
{
	internal ServiceImplementation(
		TService? implementationInstance = null,
		Type? implementationType = null,
		Type? bindingType = null,
		Func<IServiceProvider, TService>? implementationFactory = null)
	{
		Preconditions.Requires(
			implementationInstance is not null
			|| implementationType is not null
			|| bindingType is not null
			|| implementationFactory is not null,
			"At least one way of service implementation must be provided."
		);

		_implementationInstance = implementationInstance;
		_implementationType = implementationType;
		_bindingType = bindingType;
		_implementationFactory = implementationFactory;
	}

	private readonly TService? _implementationInstance;
	private readonly Type? _implementationType;
	private readonly Type? _bindingType;
	private readonly Func<IServiceProvider, TService>? _implementationFactory;

	public ServiceDescriptor ToServiceDescriptor(ServiceLifetime lifetime)
	{
		if (_implementationInstance is not null)
		{
			return new ServiceDescriptor(typeof(TService), _implementationInstance);
		}
		else if (_implementationType is not null)
		{
			return new ServiceDescriptor(typeof(TService), _implementationType, lifetime);
		}
		else if (_bindingType is not null)
		{
			return new ServiceDescriptor(typeof(TService), e => e.GetRequiredService(_bindingType), lifetime);
		}
		else if (_implementationFactory is not null)
		{
			return new ServiceDescriptor(typeof(TService), _implementationFactory, lifetime);
		}
		else
		{
			return Contracts.UnreachableReturn<ServiceDescriptor>();
		}
	}
}