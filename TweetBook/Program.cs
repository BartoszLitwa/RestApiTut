using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Data;

namespace TweetBook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Split the host builder 
            var host = CreateHostBuilder(args).Build();

            // Get the services Scope
            using (var serviceScope = host.Services.CreateScope())
            {
                // Get the dbContext from Service Provider
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

                // Run the Database ef migrations before every start
                // DO NOT use it on production build
                await dbContext.Database.MigrateAsync();

                // Get role Mangaer 
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Add roles after migartions
                if(!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    var adminRole = new IdentityRole(Roles.Admin);
                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync(Roles.Poster))
                {
                    var posterRole = new IdentityRole(Roles.Poster);
                    await roleManager.CreateAsync(posterRole);
                }
            }

            // Run the final host functions
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
