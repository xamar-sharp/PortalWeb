using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
namespace PortalWeb
{
    public class Program
    {
        public static Constraints _singleTone;
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using(IServiceScope scope = host.Services.CreateScope())
            {
                RoleManager<CustomRole> manager = scope.ServiceProvider.GetRequiredService<RoleManager<CustomRole>>();
                if (!await manager.RoleExistsAsync("USER"))
                {
                    IdentityResult res = await manager.CreateAsync(new CustomRole() { Name = "USER" });
                }
                if(!await manager.RoleExistsAsync("ADMIN"))
                {
                    IdentityResult res = await manager.CreateAsync(new CustomRole() { Name = "ADMIN" });
                }
            }
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
