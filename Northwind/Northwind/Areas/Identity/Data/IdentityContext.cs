using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Northwind.Data
{
    public class IdentityContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;
        public IdentityContext(DbContextOptions<IdentityContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var AdminId = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
            var RoleId = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

            var adminConfig = _configuration.GetSection("UserAdmin");
            var userName = adminConfig["UserName"];
            var password = adminConfig["Password"];
            var userRole = adminConfig["Role"];


            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = userRole, NormalizedName = userRole.ToUpper() });


            //create user
            var appUser = new IdentityUser
            {
                Id = AdminId,
                Email = userName,
                EmailConfirmed = true,
                UserName = userName
            };

            //set user password
            var ph = new PasswordHasher<IdentityUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, password);

            //seed user
            builder.Entity<IdentityUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = RoleId,
                UserId = AdminId
            });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
