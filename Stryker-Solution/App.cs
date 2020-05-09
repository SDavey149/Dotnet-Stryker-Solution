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
        
        public App(IOptions<Configuration> configuration)
        {
            this.configuration = configuration.Value;
        }
        
        public void Run()
        {
            string fullReportPath = this.configuration.FullReport;
            string json = File.ReadAllText(fullReportPath);

            var jObject = JObject.Parse(json);
            var files = (jObject.SelectToken("files") ?? throw new Exception("Files array is null")).Value<JObject>();
            
        }
    }
}