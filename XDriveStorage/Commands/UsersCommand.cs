using CommandDotNet;
using CommandDotNet.Prompts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XDriveStorage.Attributes;
using XDriveStorage.Configuration;
using XDriveStorage.Drives;
using XDriveStorage.Users;

namespace XDriveStorage.Commands;

[Command("users")]
public class UsersCommand
{
    [DefaultCommand]
    public int Execute()
    {
        foreach (var user in Program.AppConfiguration.Users)
        {
            if (Program.Verbose)
            {
                Output.WriteLine(user);   
            }
            else
            {
                Output.WriteLine(user.Id);
            }
        }
        
        if (Program.AppConfiguration.Users.Count == 0)
        {
            Output.WriteLine("No users to display. Add one with 'xdrive users add'.");
        }
        
        return 0;
    }

    [Command("add")]
    public int Add(
        IPrompter prompt,
        [Option("id")] [ArgumentMissingPrompt("ID:")] string id,
        [Option('d', "drive")] [ArgumentMissingPrompt("Drive:")] string drive,
        [Option('c', "creds")] JObject? credentials)
    {
        credentials ??= new JObject();

        if (!Program.AppConfiguration.Drives.Exists(drive))
        {
            Output.WriteLine($"The drive '{drive}' does not exist.");

            return 1;
        }
        
        if (!Program.AppConfiguration.Drives.TryGet(drive, out var driveObject))
        {
            Output.WriteLine($"Failed to retrieve drive '{drive}'.");

            return 1;
        }

        foreach (var credential in driveObject.Credentials.Where(c => !credentials.ContainsKey(c)))
        {
            var credentialInput = prompt.PromptForValue($"Input Credential - {credential}", out var cancel);

            if (cancel)
                return 1;

            try
            {
                credentials[credential] = JToken.Parse(credentialInput ?? string.Empty);
            }
            catch (JsonReaderException)
            {
                Output.WriteLine("Parse failed with JsonReaderException, converting input to JSON string.");

                credentials[credential] = credentialInput;
            }
        }
        
        var user = new User(id, drive, new UserCredentials(credentials));

        if (Program.AppConfiguration.Users.Exists(user.Id))
        {
            Output.WriteLine($"The user '{id}' already exists.");

            return 1;
        }
        
        if (!Program.AppConfiguration.Users.Add(user))
        {
            Output.WriteLine($"Failed to add user '{id}'.");

            return 1;
        }
        
        return 0;
    }
    
    [Command("remove")]
    public int Remove(
        string id)
    {
        if (!Program.AppConfiguration.Users.Exists(id))
        {
            Output.WriteLine($"The user '{id}' does not exist.");

            return 1;
        }
        
        if (Program.AppConfiguration.Users.Remove(id))
        {
            Output.WriteLine($"Removed user '{id}'.");

            return 0;
        }
        else
        {
            Output.WriteLine($"Failed to remove user '{id}'.");

            return 1;
        }
    }
}