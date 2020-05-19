using Microsoft.Extensions.DependencyInjection;

namespace Eleven.Core.ApplicationCore.Model.Application.Infrastructure.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection services);
    }
}
