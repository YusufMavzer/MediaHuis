using System;
using Microsoft.Extensions.DependencyInjection;

namespace MediaHuis.Notifications.WorkerService
{
    public static class IoC
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}