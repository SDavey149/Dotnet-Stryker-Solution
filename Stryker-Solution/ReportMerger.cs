using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public class ReportMerger : IReportMerger
    {
        private const string MUTANTS = "mutants";
        private readonly Configuration config;

        public ReportMerger(IOptions<Configuration> options)
        {
            config = options.Value;
        }
        
        public void MergeFilesReport(string report, JObject existingFiles)
        {
            JObject jObject = JObject.Parse(report);
            var reportFiles = (jObject.SelectToken("files") ?? throw new Exception("Files array is null"))
                .Value<JObject>();

            foreach (KeyValuePair<string, JToken> file in reportFiles)
            { 
                if (config.ExcludeFileNamesContaining.Any(x => file.Key.Contains(x)))
                {
                    return;
                }

                IncludeFile(existingFiles, file);
            }
        }

        private static void IncludeFile(JObject existingFiles, KeyValuePair<string, JToken> file)
        {
            if (existingFiles.ContainsKey(file.Key))
            {
                ReplaceFile(existingFiles, existingFiles.Value<JToken>(file.Key), file);
            }
            else
            {
                InsertNewFile(existingFiles, file);
            }
        }

        private static void InsertNewFile(JObject existingFiles, KeyValuePair<string, JToken> file)
        {
            existingFiles.Add(file.Key, file.Value);
        }

        private static void ReplaceFile(JObject existingFiles, JToken existingFile, KeyValuePair<string, JToken> file)
        {
            if (existingFile!.Value<JArray>(MUTANTS).Count(IsKilled) >= file.Value.Value<JArray>(MUTANTS).Count(IsKilled))
            {
                return;
            }
            
            existingFiles[file.Key] = file.Value;
        }

        private static bool IsKilled(JToken arg)
        {
            return arg.Value<string>("status").ToLower() == "killed";
        }
    }
}