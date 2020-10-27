using Microsoft.Extensions.DependencyInjection;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Repositories;

namespace Northwind.DataAccess.Infrastructure
{
    public static class DataRepositoryExtentions
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            //services.AddScoped<DbContext, DataContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
