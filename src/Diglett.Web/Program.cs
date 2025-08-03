using Autofac.Extensions.DependencyInjection;
using Diglett.Core.Catalog.Search;
using Diglett.Core.Catalog.Search.Modelling;
using Diglett.Core.Data;
using Diglett.Core.Domain.Identity;
using Diglett.Core.Engine;
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

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            var services = builder.Services;
            var appContext = new DiglettApplicationContext();
            var engine = EngineFactory.Create(appContext);

            services.AddSingleton(appContext as IApplicationContext);

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddDbContext<DiglettDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<DiglettDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<TimestampedInterceptor>();
            services.AddScoped<ICatalogSearchService, CatalogSearchService>();
            services.AddScoped<ICatalogSearchQueryFactory, CatalogSearchQueryFactory>();
            services.AddScoped<IWebHelper, DefaultWebHelper>();
            services.AddScoped<CatalogHelper>();

            var app = builder.Build();

            var providerContainer = appContext as IServiceProviderContainer;
            providerContainer.ApplicationServices = app.Services;

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
