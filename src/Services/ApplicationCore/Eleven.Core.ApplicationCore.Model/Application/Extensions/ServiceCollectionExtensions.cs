using Microsoft.Extensions.DependencyInjection;
using Eleven.Core.ApplicationCore.Model.Application.Infrastructure.IoC;

namespace Eleven.Core.ApplicationCore.Model.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(services);
            }

            return ServiceTool.Create(services);
        }
    }
}
