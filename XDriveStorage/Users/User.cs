using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XDriveStorage.Users;

public class User
{
    public string Id { get; }
    public string Drive { get; }
    public UserCredentials Credentials { get; }

    public User(string id, string drive, UserCredentials credentials)
    {
        Id = id;
        Drive = drive;
        Credentials = credentials;
    }

    public override string ToString()
    {
        return JObject.FromObject(this).ToString(Formatting.Indented);
    }
}