using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var UserContext = services.GetRequiredService<UserContext>();
                    UserDbInitializer.Initialize(UserContext);

                    var OrderContext = services.GetRequiredService<OrderContext>();
                    OrderDbInitializer.Initialize(OrderContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                
                .UseStartup<Startup>()
                .UseUrls("http://*:9000")
                //.UseUrls("https://*:9000")
                .Build();
    }
}