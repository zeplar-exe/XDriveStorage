using System.Numerics;
using System.Reflection;

using XDriveStorage.Drives.Builtin;
using XDriveStorage.Users;

namespace XDriveStorage.Drives;

public interface IDrive
{
    public string Name { get; }
    public DriveCredentials Credentials { get; }
    public DriveConfiguration Configuration { get; }

    public Task<StorageLimit> GetStorageLimit(UserCredentials userCredentials);
    public Task<string[]> ListFileNames(UserCredentials credentials);
    public Task<bool> ReadFile(UserCredentials credentials, string name, Stream outputStream);
    public Task<bool> WriteFile(UserCredentials credentials, string name, Stream content);
    public Task<bool> DeleteFile(UserCredentials credentials, string name);

    public static bool IsBuiltin(IDrive drive)
    {
        return IsBuiltin(drive.GetType());
    }
    
    public static bool IsBuiltin<T>() where T : IDrive
    {
        return IsBuiltin(typeof(T));
    }
    
    public static bool IsBuiltin(Type type)
    {
        return type.IsAssignableTo(typeof(IDrive)) && type.GetCustomAttribute<BuiltinDriveAttribute>() != null;
    }
}