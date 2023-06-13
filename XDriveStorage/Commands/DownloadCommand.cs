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