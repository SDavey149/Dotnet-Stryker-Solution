using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Stryker_Solution
{
    public class ProjectProvider : IProjectProvider
    {
        private readonly Configuration config;
        
        public string[] TestProjectPaths { get; }
        
        public string[] SourceProjects { get; }
        
        public ProjectProvider(IOptions<Configuration> config)
        {
            this.config = config.Value;
            string[] testProjects = GetTestProjects();
            TestProjectPaths = testProjects
                .Select(x => x.Substring(0,x.LastIndexOf('\\')))
                .ToArray();
            SourceProjects = GetSourceProjects(testProjects);
        }
        
        private string[] GetSourceProjects(string[] testProjects)
        {
            string[] allProjects = GetFilesWithPattern("*.csproj");
            var projects = new HashSet<string>(allProjects);
            var testSet = new HashSet<string>(testProjects);
            return projects.Except(testSet).ToArray();
        }

        private string[] GetTestProjects()
        {
            return GetFilesWithPattern(config.TestProjectFormat);
        }
        
        private string[] GetFilesWithPattern(string pattern)
        {
            return Directory.GetFiles(config.SolutionDirectory, pattern, SearchOption.AllDirectories);
        }
    }
}