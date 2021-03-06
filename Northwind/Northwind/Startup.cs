using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web.UI;
using Northwind.Infrastructure;
using Northwind.Models;
using Serilog;
using System.Net;
using System.Reflection;

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
            var logger = new LoggerConfiguration()
                      .ReadFrom.Configuration(Configuration)
                      .CreateLogger();
            services.AddSingleton<ILogger>(logger);
            logger.Information("Starting configuration");

            services.AddWebApplication();
            services.AddResponseCaching();
            services.AddDatabase(Configuration);
            services.AddBusiness();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedEmail = false;
            });

            services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                    sharedOptions.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                    sharedOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.Configure<OpenIdConnectOptions>(AzureADDefaults.AuthenticationScheme, options =>
            {
                options.UseTokenLifetime = false;
            });

            services.AddControllers();

            services.AddRazorPages()
                .AddMicrosoftIdentityUI();
            logger.Information("Application configured");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger logger, IConfiguration configuration, UserManager<IdentityUser> userManager)
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
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
