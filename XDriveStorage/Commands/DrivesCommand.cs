using CommandDotNet;
using CommandDotNet.Prompts;

using Newtonsoft.Json.Linq;

using XDriveStorage.Attributes;
using XDriveStorage.Configuration;
using XDriveStorage.Drives;

namespace XDriveStorage.Commands;

[Command("drives")]
public class DrivesCommand
{
    private IAppConfiguration AppConfiguration { get; }

    public DrivesCommand(IAppConfiguration appConfiguration)
    {
        AppConfiguration = appConfiguration;
    }

    [DefaultCommand]
    public void Execute()
    {
        foreach (var drive in AppConfiguration.Drives)
        {
            if (Program.Verbose)
            {
                Console.WriteLine(drive);
            }
            else
            {
                Console.WriteLine(drive.Name);
            }
        }
    }
}