using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public interface IFullReportProducer
    {
        void Process(JObject files);
    }
}