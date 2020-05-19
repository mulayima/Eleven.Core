using System;
using Microsoft.Extensions.DependencyInjection;

namespace Eleven.Core.ApplicationCore.Model.Application.Infrastructure.IoC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
