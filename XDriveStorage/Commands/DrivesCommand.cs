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
    [DefaultCommand]
    public void Execute()
    {
        foreach (var drive in Program.AppConfiguration.Drives)
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

        if (Program.AppConfiguration.Drives.Count == 0)
        {
            Console.WriteLine("No drives to display... that shouldn't be possible. Maybe reinstall?");
        }
    }
}