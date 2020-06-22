using System.IO;

namespace Stryker_Solution
{
    public class Configuration
    {
        private string solutionDirectory;
        
        public string SolutionDirectory
        {
            get => solutionDirectory;
            set
            {
                solutionDirectory = value.Replace("/", "\\");
                
                if (!Path.IsPathFullyQualified(solutionDirectory))
                {
                    solutionDirectory = Path.GetFullPath(solutionDirectory);
                }
            }
        }

        public string[] ExcludeFileNamesContaining { get; set; }
        
        public string TestProjectFormat { get; set; }
    }
}