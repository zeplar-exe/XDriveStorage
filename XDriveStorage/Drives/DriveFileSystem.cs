using System.Numerics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XDriveStorage.Configuration;
using XDriveStorage.Users;

namespace XDriveStorage.Drives;

public class DriveFileSystem
{
    private const string RootFileName = "00_root.json";

    private IDrive RootDrive { get; }
    private RootDriveData RootDriveData { get; }

    public static async Task<DriveFileSystem?> Create()
    {
        var rootUserId = Program.AppConfiguration.RootUserId;

        if (rootUserId == null)
        {
            Output.WriteError("The root user has not been set.");

            return null;
        }

        if (!Program.AppConfiguration.Users.TryGet(rootUserId, out var rootUser))
        {
            Output.WriteError($"Failed to get root user '{rootUserId}'.");
            
            return null;
        }

        if (!Program.AppConfiguration.Drives.TryGet(rootUser.Drive, out var rootDrive))
        {
            Output.WriteError($"The root drive '{rootUser.Drive}' does not exist.");
            
            return null;
        }

        using var rootDriveStream = new MemoryStream();

        if (await rootDrive.ReadFile(rootUser.Credentials, RootFileName, rootDriveStream))
        {
            var json = await JObject.LoadAsync(new JsonTextReader(new StreamReader(rootDriveStream)));
            var rootDriveData = json.ToObject<RootDriveData>()!;
            
            return new DriveFileSystem(rootDrive, rootDriveData);
        }

        return null;
    }

    private DriveFileSystem(IDrive rootDrive, RootDriveData rootDriveData)
    {
        RootDrive = rootDrive;
        RootDriveData = rootDriveData;
    }

    public async Task ReadFile(string name, Stream outputStream)
    {
        
    }

    public async Task WriteFile(string name, Stream content)
    {
        
    }

    public async Task DeleteFile(string name)
    {
        
    }
}