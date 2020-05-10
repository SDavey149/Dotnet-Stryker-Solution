using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public class App
    {
        private readonly Configuration configuration;
        private readonly IFullReportProducer fullReportProducer;
        private readonly IStrykerRunner strykerRunner;
        
        public App(IOptions<Configuration> configuration,
            IStrykerRunner strykerRunner,
            IFullReportProducer fullReportProducer)
        {
            this.configuration = configuration.Value;
            this.strykerRunner = strykerRunner;
            this.fullReportProducer = fullReportProducer;
        }
        
        public void Run()
        {
            JObject fullReport = strykerRunner.Run();
            fullReportProducer.Process(fullReport);
        }
    }
}