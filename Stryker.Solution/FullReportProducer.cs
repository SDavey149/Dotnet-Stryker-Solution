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
    }
}