using System.Diagnostics;

namespace Stryker_Solution
{
    public class CommandRunner : ICommandRunner
    {
        public string RunShellCommand(string command)
        {
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/c {command}";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}