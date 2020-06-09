using System;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public class FullReportProducer : IFullReportProducer
    {
        private readonly Configuration config;
        
        public FullReportProducer(IOptions<Configuration> config)
        {
            this.config = config.Value;
        }
        
        public void Process(JObject files)
        {
            ApplyExclusions(files);

            string jsonOutput = files.ToString();
            WriteJsonReport(jsonOutput);
            WriteHtmlReport(jsonOutput);
        }

        private void WriteJsonReport(string jsonOutput)
        {
            File.WriteAllText($"{config.SolutionDirectory}\\full-report.json",
                jsonOutput);
        }

        private void WriteHtmlReport(string jsonOutput)
        {
            var htmlTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "stryker-empty.html");
            var htmlReport = htmlTemplate.Replace("##REPORT_JSON##", jsonOutput);
            File.WriteAllText($"{config.SolutionDirectory}\\full-report.html", htmlReport);
        }

        private static void ApplyExclusions(JObject files)
        {
            var properties = files.Properties();
            foreach (JProperty property in properties)
            {
                var sourceVal = property.Value.SelectToken("source");
                if (sourceVal is null)
                {
                    continue;
                }
                
                var source = sourceVal.Value<string>();
            }
        }
    }
}