using CommandDotNet;

namespace XDriveStorage.Commands;

[Command("download")]
public class DownloadCommand
{
    [DefaultCommand]
    public int Execute(
        string driveFileTarget, 
        [Option('o', "output")] string? outputFile)
    {
        return 0;
    }
}

[Command("download-many")]
public class DownloadManyCommand
{
    [DefaultCommand]
    public int Execute(
        string[] driveFileTargets, 
        [Option('o', "outputs")] string[]? outputFiles)
    {
        if (outputFiles != null && driveFileTargets.Length != outputFiles.Length)
        {
            Output.WriteError($"Argument mismatch. Expected {driveFileTargets.Length} output file targets to match input files, got {outputFiles.Length} instead.");

            return 1;
        }
        
        return 0;
    }
}