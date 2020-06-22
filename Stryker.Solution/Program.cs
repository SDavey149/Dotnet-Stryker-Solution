using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stryker_Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            SetConfigValues(serviceCollection, args.FirstOrDefault());
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
        }

        private static void SetConfigValues(IServiceCollection serviceCollection, string solution)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile(Directory.GetCurrentDirectory() + "\\stryker-solution-config.json", false)
                .Build();
            
            IConfigurationSection section = configuration.GetSection("Config");

            if (solution != null)
            {
                section["SolutionDirectory"] = solution;
            }
            
            serviceCollection.Configure<Configuration>(section);
        }
        
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<App>()
                .AddSingleton<IFullReportProducer, FullReportProducer>()
                .AddSingleton<IProjectProvider, ProjectProvider>()
                .AddSingleton<IStrykerRunner, StrykerRunner>()
                .AddSingleton<ICommandRunner, CommandRunner>()
                .AddSingleton<IReportMerger, ReportMerger>();
        }
    }
}