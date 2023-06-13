using CommandDotNet;

namespace XDriveStorage.Commands;

[Command("root")]
public class RootCommand
{
    [DefaultCommand]
    public int Execute()
    {
        return 0;
    }
    
    [Command("set")]
    public int Set(string userId)
    {
        return 0;
    }
}