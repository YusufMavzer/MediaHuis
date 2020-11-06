using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace MediaHuis.Notifications.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    services.AddSingleton(new Config()
                    {
                        AirshipToken = configuration.GetSection("AirshipToken").Value,
                        RabbitMqHostName = configuration.GetSection("RabbitMqHost").Value
                    });
                    services.AddDbContext<NotificationContext>(o => 
                        o.UseSqlServer(configuration.GetSection("NotificationContext").Value)
                        );
                    services.AddSingleton<AirshipClient>();
                    services.AddHostedService<Worker>();
                    IoC.ServiceProvider = services.BuildServiceProvider();
                })
                .UseConsoleLifetime();
            return host;
        }
    }
}