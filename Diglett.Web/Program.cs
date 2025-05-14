using Diglett.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Diglett.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;

            services.AddControllersWithViews();

            services.AddDbContext<DiglettDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

            services.AddScoped<TimestampedInterceptor>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}