using System.Collections.Immutable;
using Common.Core.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection;

public class DescriptorsServicesPlugin : IServicesPlugin
{
	protected IImmutableList<ServiceDescriptor> Descriptors { get; }

	public DescriptorsServicesPlugin(ServiceLifetime lifetime, params (Type, Type)[] pairs)
	{
		Preconditions.RequiresNotNull(pairs, nameof(pairs));

		Descriptors = pairs.Select(e => new ServiceDescriptor(e.Item1, e.Item2, lifetime)).ToImmutableList();
	}

	public DescriptorsServicesPlugin(params (Type, Type, ServiceLifetime)[] pairs)
	{
		Preconditions.RequiresNotNull(pairs, nameof(pairs));

		Descriptors = pairs.Select(e => new ServiceDescriptor(e.Item1, e.Item2, e.Item3)).ToImmutableList();
	}

	public DescriptorsServicesPlugin(IImmutableList<ServiceDescriptor> descriptors)
	{
		Preconditions.RequiresNotNull(descriptors, nameof(descriptors));

		Descriptors = descriptors;
	}

	public virtual void InstallTo(IServiceCollection services)
	{
		Preconditions.RequiresNotNull(services, nameof(services));

		foreach (var descriptor in Descriptors)
		{
			services.Add(descriptor);
		}
	}
}