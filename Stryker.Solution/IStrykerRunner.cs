using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public interface IStrykerRunner
    {
        JObject Run();
    }
}