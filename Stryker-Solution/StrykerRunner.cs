using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public class StrykerRunner : IStrykerRunner
    {
        private const string STRYKER_REF_ERROR = "Project reference issue";
        private const string MUTANT = "mutants";

        private readonly IProjectProvider projectProvider;
        private readonly ICommandRunner commandRunner;
        
        public StrykerRunner(IProjectProvider projectProvider, 
            ICommandRunner commandRunner)
        {
            this.projectProvider = projectProvider;
            this.commandRunner = commandRunner;
        }
        
        public JObject Run()
        {
            var allFiles = new JObject();
            foreach (string testProjectPath in projectProvider.TestProjectPaths)
            {
                foreach (string projectToMutate in projectProvider.SourceProjects)
                {
                    string report = RunStryker(testProjectPath, projectToMutate);
                    if (string.IsNullOrEmpty(report))
                    {
                        continue;
                    }
                    
                    JObject jObject = JObject.Parse(report);
                    var reportFiles = (jObject.SelectToken("files") ?? throw new Exception("Files array is null"))
                        .Value<JObject>();

                    Console.WriteLine("Files count: " + reportFiles.Count);
                    foreach (KeyValuePair<string,JToken> file in reportFiles)
                    {
                        bool fileExists = allFiles.TryGetValue(file.Key, out JToken existingFile);
                        if (fileExists)
                        {
                            if (existingFile.Value<JArray>(MUTANT).Count < 
                                file.Value.Value<JArray>(MUTANT).Count)
                            {
                                allFiles[file.Key] = file.Value;
                            }
                        }
                        else
                        {
                            allFiles.Add(file.Key, file.Value);
                        }
                    }
                }
            }

            return allFiles;
        }

        private string RunStryker(string testPath, string projectToMutate)
        {
            string command = $"cd {testPath} && " +
                             $"dotnet stryker --reporters \"['json']\" --project-file={projectToMutate}";

            string output = commandRunner.RunShellCommand(command);

            if (output.Contains(STRYKER_REF_ERROR))
            {
                return string.Empty;
            }

            return ReadReport(testPath);
        }

        private static string ReadReport(string testPath)
        {
            var strykerOutput = $"{testPath}\\StrykerOutput";
            DirectoryInfo latestOutput = new DirectoryInfo(strykerOutput).GetDirectories()
                .OrderByDescending(d=>d.LastWriteTimeUtc).First();

            string reportPath = $"{latestOutput.FullName}\\reports\\mutation-report.json";
            return File.ReadAllText(reportPath);
        }
        
    }
}