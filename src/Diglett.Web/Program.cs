using Diglett.Core.Catalog.Search;
using Diglett.Core.Data;
using Diglett.Core.Domain.Identity;
using Diglett.Core.Web;
using Diglett.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddDbContext<DiglettDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<DiglettDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<TimestampedInterceptor>();
            services.AddScoped<ICatalogSearchService, CatalogSearchService>();
            services.AddScoped<IWebHelper, DefaultWebHelper>();
            services.AddScoped<CatalogHelper>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DiglettDbContext>().Database.Migrate();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();
            app.UseStaticFiles();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
