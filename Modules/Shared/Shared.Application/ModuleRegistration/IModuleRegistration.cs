using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.ModuleRegistration;

public interface IModuleRegistration
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
    void RegisterDbContext(IServiceCollection services, IConfiguration configuration);
    void RegisterMediatR(IServiceCollection services);
    void RegisterControllers(IServiceCollection services);
}
