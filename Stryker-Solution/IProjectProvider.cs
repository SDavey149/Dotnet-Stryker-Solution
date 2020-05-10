namespace Stryker_Solution
{
    public interface IProjectProvider
    {
        string[] TestProjectPaths { get; }
        string[] SourceProjects { get; }
    }
}