using Microsoft.Extensions.DependencyInjection;
using Northwind.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
