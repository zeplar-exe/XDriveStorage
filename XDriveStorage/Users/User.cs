using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XDriveStorage.Users;

public class User
{
    [JsonProperty("id")] public string Id { get; private set; }
    [JsonProperty("drive")] public string Drive { get; private set; }
    [JsonProperty("credentials")] public UserCredentials Credentials { get; private set; }

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