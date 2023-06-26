using System.Numerics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XDriveStorage.Configuration;
using XDriveStorage.Users;

namespace XDriveStorage.Drives;

public class DriveFileSystem
{
    private RootDriveData RootDriveData { get; }

    public static async Task<DriveFileSystem?> Create(User rootUser)
    {
        const string RootFileName = "0_root.json";
        
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
        RootDriveData = rootDriveData;
    }

    public void ReadFile(string name, Stream outputStream)
    {
        
    }

    public void WriteFile(string name, Stream content)
    {
        
    }

    public void DeleteFile(string name)
    {
        
    }
}