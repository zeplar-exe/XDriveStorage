using CommandDotNet;

using XDriveStorage.Drives;

namespace XDriveStorage.Commands;

[Command("upload")]
public class UploadCommand
{
    [DefaultCommand]
    public async Task<int> Execute(
        string inputFile, 
        [Option('t', "target")] string? driveFileTarget)
    {
        driveFileTarget ??= inputFile;
        
        using var fileStream = File.OpenRead(inputFile);
        
        var fileSystem = await DriveFileSystem.Create();

        if (fileSystem == null)
        {
            Output.WriteError("Failed to create drive file system.");
            
            return 1;
        }
        
        fileSystem.WriteFile(driveFileTarget, fileStream).Wait();
        
        return 0;
    }
}

[Command("upload-many")]
public class UploadManyCommand
{
    [DefaultCommand]
    public async Task<int> Execute(
        string[] inputFiles, 
        [Option('t', "targets")] string[]? driveFileTargets)
    {
        if (driveFileTargets != null && inputFiles.Length != driveFileTargets.Length)
        {
            Output.WriteError($"Argument mismatch. Expected {inputFiles.Length} file targets to match input files, got {driveFileTargets.Length} instead.");

            return 1;
        }
        
        var fileSystem = await DriveFileSystem.Create();

        if (fileSystem == null)
        {
            Output.WriteError("Failed to create drive file system.");
            
            return 1;
        }

        var tasks = new List<Task>();

        for (var i = 0; i < inputFiles.Length; i++)
        {
            var inputFile = inputFiles[i];
            var driveFile = driveFileTargets?[i] ?? inputFile;
            
            tasks.Add(WriteSingleFile(fileSystem, inputFile, driveFile));
        }

        Task.WaitAll(tasks.ToArray());
        
        return 0;
    }

    private async Task WriteSingleFile(DriveFileSystem fileSystem, string inputFile, string driveFile)
    {
        await using var fileStream = File.OpenRead(inputFile);

        await fileSystem.WriteFile(driveFile, fileStream);
    }
}