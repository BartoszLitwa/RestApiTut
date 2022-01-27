using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
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
            using (var serviceScopes = host.Services.CreateScope())
            {
                // Get the dbContext from Service Provider
                var dbContext = serviceScopes.ServiceProvider.GetRequiredService<DataContext>();

                // Run the Database ef migrations before every start
                // DO NOT use it on production build
                await dbContext.Database.MigrateAsync();
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
