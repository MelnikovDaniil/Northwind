using Microsoft.Extensions.DependencyInjection;
using Northwind.Business.Interfaces;
using Northwind.Business.Services;

namespace Northwind.Infrastructure
{
    public static class BusinessModule
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISupplierService, SupplierService>();
            return services;
        }
    }
}
