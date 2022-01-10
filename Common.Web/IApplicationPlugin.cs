using Common.DependencyInjection;

namespace Common.Web;

public interface IApplicationPlugin : IServicesPlugin
{
	void UseIn(IApplicationBuilder applicationBuilder);
}