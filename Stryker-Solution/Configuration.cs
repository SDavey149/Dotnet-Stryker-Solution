namespace Stryker_Solution
{
    public class Configuration
    {
        private string solutionDirectory;
        
        public string SolutionDirectory
        {
            get => solutionDirectory;
            set => solutionDirectory = value.Replace("/", "\\");
        }
        
        public string[] ExcludeFileNamesContaining { get; set; }
        
        public string TestProjectFormat { get; set; }
    }
}