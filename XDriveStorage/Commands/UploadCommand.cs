using CommandDotNet;

using XDriveStorage.Drives;

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