using FinanceME.Data;
using FinanceME.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceME
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Pull the connection string from appsettings — crash early if it's missing
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Wire Identity to our custom User class.
            // We use AddIdentity (not AddDefaultIdentity) so we get role support built in.
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // Turn this off during development — you don't want to need a working
                // email sender just to test a login. Flip it back to true before deploying.
                options.SignIn.RequireConfirmedAccount = false;

                // Password rules
                options.Password.RequireDigit           = true;
                options.Password.RequireLowercase       = true;
                options.Password.RequireUppercase       = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength         = 8;

                // Lock the account for 5 minutes after 5 wrong password attempts
                options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers      = true;

                // No two accounts can share the same email address
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI()            // Brings in the built-in login/register/reset pages
            .AddDefaultTokenProviders(); // Needed for password reset and email confirmation tokens

            // Cookie settings — how long a session lasts and where to redirect on auth failures
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly   = true;
                options.ExpireTimeSpan    = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true; // Extends the 60-min window whenever the user is active

                options.LoginPath        = "/Identity/Account/Login";
                options.LogoutPath       = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Authentication must be registered before Authorization — order matters here
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); // Required for the Identity UI pages to work

            app.Run();
        }
    }
}
