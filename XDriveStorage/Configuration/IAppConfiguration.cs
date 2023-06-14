using Newtonsoft.Json.Linq;

using XDriveStorage.Drives;
using XDriveStorage.Users;

namespace XDriveStorage.Configuration;

public interface IAppConfiguration
{
    public string? RootUserId { get; set; }
    public UserContainer Users { get; }
    public DriveContainer Drives { get; }

    public JToken? GetPersistent(string bucket, string name);
    public void StorePersistent(string bucket, string name, JToken value);

    public void Save();
}