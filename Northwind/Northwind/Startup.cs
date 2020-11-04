using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Northwind.Infrastructure;
using Northwind.Models;
using Serilog;

namespace Northwind
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddControllersWithViews();
            services.AddDatabase(Configuration);
            services.AddBusiness();
            services.AddSingleton<ILogger>(
                context => new LoggerConfiguration()
                      .ReadFrom.Configuration(Configuration)
                      .CreateLogger());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger, IConfiguration configuration)
        {
            var applicationPath = Assembly.GetExecutingAssembly().Location;
            logger.Information("Path to application " + applicationPath);
            if (env.IsDevelopment())
            {
                logger.Information("Application run with developer regime");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.Information("Application run without error confirmation");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var productsPerPage = configuration.GetValue<int>("ProductsPerPage");
            var cacheMaxAge = configuration.GetValue<int>("CacheMaxAge");

            logger.Information($"Number of products per page:{(productsPerPage == 0 ? "unlimited" : productsPerPage.ToString())}");
            logger.Information($"Image cache max age: {cacheMaxAge}s");

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.Error($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        }.ToString());
                    }
                });
            });
        }
    }
}
