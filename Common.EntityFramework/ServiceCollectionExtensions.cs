using System.Reflection;
using System.Runtime.ExceptionServices;
using Common.Core.Diagnostics;
using Common.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.EntityFramework;

public static class ServiceCollectionExtensions
{
	private static MethodInfo _AddDbContextMethod { get; }
		= typeof(EntityFrameworkServiceCollectionExtensions).GetMethod(
			  nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext),
			  2,
			  BindingFlags.Static | BindingFlags.Public,
			  null,
			  new[] { typeof(IServiceCollection), typeof(ServiceLifetime), typeof(ServiceLifetime) },
			  null
		  )
		  ?? throw Errors.UnexpectedContract();

	public static void AddDbContext<T>(
		this IServiceCollection @this,
		ServiceImplementation<T> implementation,
		ServiceLifetime lifetime = ServiceLifetime.Scoped)
		where T : DbContext
	{
		Preconditions.RequiresNotNull(@this, nameof(@this));
		Preconditions.RequiresNotNull(implementation, nameof(implementation));

		var serviceDescriptor = implementation.ToServiceDescriptor(lifetime);

		try
		{
			_AddDbContextMethod
				.MakeGenericMethod(
					serviceDescriptor.ServiceType,
					serviceDescriptor.ImplementationType ?? serviceDescriptor.ServiceType
				)
				.Invoke(null, new object[] { @this, lifetime, ServiceLifetime.Scoped });
		}
		catch (TargetInvocationException e)
		{
			var innerException = e.InnerException;
			if (innerException is not null)
			{
				ExceptionDispatchInfo.Throw(innerException);
			}
			else
			{
				throw;
			}
		}
	}
}