using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public class FullReportProcessor
    {

        public void Process(JObject files)
        {
            ApplyExclusions(files);
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