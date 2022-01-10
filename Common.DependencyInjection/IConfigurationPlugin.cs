using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DependencyInjection;

public interface IConfigurationPlugin
{
	public void ConfigureInto(IServiceCollection services, IConfiguration configuration);
}