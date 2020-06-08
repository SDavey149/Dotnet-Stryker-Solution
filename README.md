# READ ME

This tool runs stryker on multiple projects with as little config as possible. It works in solutions with either a
single test project or multiple test projects. It tries to run all possible combinations between the source projects
and the test projects. For very large projects you might want to optimise that.

A report (full-report.html) is output into the solution directory. 

Bear in mind this is a half-assed job until Stryker implement their own functionality to run on whole 
.NET core solutions. :)

## Config

```
{
    "Config": 
    {
        "SolutionDirectory": "C:/Path/ToFolder",
        "ExcludeFileNamesContaining": [
            "Migrations",
        ]
    }
}
```