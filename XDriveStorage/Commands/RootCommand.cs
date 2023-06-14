using CommandDotNet;

namespace XDriveStorage.Commands;

[Command("root")]
public class RootCommand
{
    [DefaultCommand]
    public int Execute()
    {
        var userId = Program.AppConfiguration.RootUserId;

        if (userId == null)
        {
            Output.WriteWarning("A root user has not been set; other commands will not be able to run. Set one with 'xdrive root set'.");
        }
        else
        {
            Output.WriteLine(userId);

            if (!Program.AppConfiguration.Users.Exists(userId))
            {
                Output.WriteWarning($"The root user '{userId}' does not exist; other operations may fail. Change it or create a user with this id.");
            }
        }
        
        return 0;
    }
    
    [Command("set")]
    public int Set(string userId)
    {
        Program.AppConfiguration.RootUserId = userId;
        
        if (!Program.AppConfiguration.Users.Exists(userId))
        {
            Output.WriteWarning($"The root user '{userId}' does not exist; other operations may fail. Change it or create a user with this id.");
        }
    
        return 1;
    }
}