using CommandDotNet;

using XDriveStorage.Drives;

namespace XDriveStorage.Commands;

[Command("download")]
public class DownloadCommand
{
    [DefaultCommand]
    public async Task<int> Execute(
        string driveFileTarget, 
        [Option('o', "output")] string? outputFile)
    {
        outputFile ??= driveFileTarget;
        
        using var fileStream = File.OpenWrite(outputFile);
        
        var fileSystem = await DriveFileSystem.Create();

        if (fileSystem == null)
        {
            Output.WriteError("Failed to create drive file system.");
            
            return 1;
        }
        
        fileSystem.ReadFile(driveFileTarget, fileStream).Wait();
        fileStream.Flush();
        
        return 0;
    }
}

[Command("download-many")]
public class DownloadManyCommand
{
    [DefaultCommand]
    public async Task<int> Execute(
        string[] driveFileTargets, 
        [Option('o', "outputs")] string[]? outputFiles)
    {
        if (outputFiles != null && driveFileTargets.Length != outputFiles.Length)
        {
            Output.WriteError($"Argument mismatch. Expected {driveFileTargets.Length} output file targets to match input files, got {outputFiles.Length} instead.");

            return 1;
        }
        
        var fileSystem = await DriveFileSystem.Create();

        if (fileSystem == null)
        {
            Output.WriteError("Failed to create drive file system.");
            
            return 1;
        }

        var tasks = new List<Task>();

        for (var i = 0; i < driveFileTargets.Length; i++)
        {
            var driveFile = driveFileTargets[i];
            var outputFile = outputFiles?[i] ?? driveFile;
            
            tasks.Add(ReadSingleFile(fileSystem, driveFile, outputFile));
        }

        Task.WaitAll(tasks.ToArray());
        
        return 0;
    }

    private async Task ReadSingleFile(DriveFileSystem fileSystem, string driveFile, string outputFile)
    {
        await using var fileStream = File.OpenWrite(outputFile);
        
        await fileSystem.ReadFile(driveFile, fileStream);
    }
}