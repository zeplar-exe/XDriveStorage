using CommandDotNet;

namespace XDriveStorage.Commands;

[Command("upload")]
public class UploadCommand
{
    [DefaultCommand]
    public int Execute(
        string inputFile, 
        [Option('t', "target")] string? driveFileTarget)
    {
        return 0;
    }
}

[Command("upload-many")]
public class UploadManyCommand
{
    [DefaultCommand]
    public int Execute(
        string[] inputFiles, 
        [Option('t', "targets")] string[]? driveFileTargets)
    {
        if (driveFileTargets != null && inputFiles.Length != driveFileTargets.Length)
        {
            Output.WriteError($"Argument mismatch. Expected {inputFiles.Length} file targets to match input files, got {driveFileTargets.Length} instead.");

            return 1;
        }
        
        return 0;
    }
}