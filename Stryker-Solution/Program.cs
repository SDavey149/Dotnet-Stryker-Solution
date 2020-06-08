using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stryker_Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
        }
        
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("stryker-solution-config.json", false)
                .Build();
            
            IConfigurationSection section = configuration.GetSection("Config");
            serviceCollection.Configure<Configuration>(section);
            
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddTransient<App>();
            serviceCollection.AddSingleton<IFullReportProducer, FullReportProducer>();
            serviceCollection.AddSingleton<IProjectProvider, ProjectProvider>();
            serviceCollection.AddSingleton<IStrykerRunner, StrykerRunner>();
            serviceCollection.AddSingleton<ICommandRunner, CommandRunner>();
            serviceCollection.AddSingleton<IReportMerger, ReportMerger>();
        }
    }
}