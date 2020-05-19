using Microsoft.Extensions.DependencyInjection;
using Eleven.Core.ApplicationCore.Model.Application.Infrastructure.IoC;

namespace Eleven.Core.ApplicationCore.Model.Application.Infrastructure.DependencyResolver
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();
            //services.AddSingleton<ICacheManager, MemoryCacheManager>(); // Need Implement
        }
    }
}
