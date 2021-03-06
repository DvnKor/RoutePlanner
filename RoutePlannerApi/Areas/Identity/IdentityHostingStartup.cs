using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoutePlannerApi.Areas.Identity;
using RoutePlannerApi.Areas.Identity.Data;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace RoutePlannerApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UsersDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("UsersDbContextConnection")));

                services.AddDefaultIdentity<RoutePlannerAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<UsersDbContext>();
            });
        }
    }
}