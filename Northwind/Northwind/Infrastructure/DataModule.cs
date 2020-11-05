using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Data.Domain;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Repositories;

namespace Northwind.Infrastructure
{
    public static class DataModule
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Northwind");

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddDbContext<NorthwindContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(connectionString);
            });
            return services;
        }
    }
}
