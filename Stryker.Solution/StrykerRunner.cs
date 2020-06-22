using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Stryker_Solution
{
    public class StrykerRunner : IStrykerRunner
    {
        private const string STRYKER_REF_ERROR = "Project reference issue";
        private const string STRYKER_SUCCESS = "final mutation score";

        private readonly IProjectProvider projectProvider;
        private readonly ICommandRunner commandRunner;
        private readonly IReportMerger reportMerger;
        
        public StrykerRunner(IProjectProvider projectProvider, 
            ICommandRunner commandRunner,
            IReportMerger reportMerger)
        {
            this.projectProvider = projectProvider;
            this.commandRunner = commandRunner;
            this.reportMerger = reportMerger;
        }
        
        public JObject Run()
        {
            var reportedTestProjects = string.Join(", ", projectProvider.TestProjectPaths);
            Console.WriteLine($"Running mutation tests on these test projects: {reportedTestProjects}");
            
            var existingFiles = new JObject();
            foreach (string testProjectPath in projectProvider.TestProjectPaths)
            {
                foreach (string projectToMutate in projectProvider.SourceProjects)
                {
                    string report = RunStryker(testProjectPath, projectToMutate);
                    if (string.IsNullOrEmpty(report))
                    {
                        continue;
                    }

                    reportMerger.MergeFilesReport(report, existingFiles);
                }
            }

            return existingFiles;
        }

        private string RunStryker(string testPath, string projectToMutate)
        {
            string command = $"cd {testPath} && " +
                             $"dotnet stryker --reporters \"['json']\" --project-file={projectToMutate}";

            string output = commandRunner.RunShellCommand(command);

            if (!output.Contains(STRYKER_SUCCESS))
            {
                if (!output.Contains(STRYKER_REF_ERROR))
                {
                    Console.WriteLine(output);
                }
                
                return string.Empty;
            }

            Console.WriteLine($"Mutated {projectToMutate} for tests {testPath}");
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