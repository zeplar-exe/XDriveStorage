using CommandDotNet;

namespace XDriveStorage.Commands;

[Command("patch")]
public class PatchCommand
{
    [DefaultCommand]
    public int Execute()
    {
        // Saving this for later when we need to patch formatting versions

        return 0;
    }
}