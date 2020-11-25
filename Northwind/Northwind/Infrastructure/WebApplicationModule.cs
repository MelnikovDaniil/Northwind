using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Filters;

namespace Northwind.Infrastructure
{
    public static class WebApplicationModule
    {
        public static IServiceCollection AddWebApplication(this IServiceCollection services)
        {
            services.AddScoped<LoggingFilter>();
            services.AddScoped<BreadcrumbActionFilter>();
            return services;
        }
    }
}
