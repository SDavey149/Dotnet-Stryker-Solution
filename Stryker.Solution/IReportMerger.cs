using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public interface IReportMerger
    {
        void MergeFilesReport(string report, JObject existingFiles);
    }
}