using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection;

public interface IServicesPlugin
{
	void InstallTo(IServiceCollection services);
}