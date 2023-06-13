using Newtonsoft.Json.Linq;

using XDriveStorage.Drives;

namespace XDriveStorage.Users;

public class UserCredentials
{
    private Dictionary<string, JToken> Credentials { get; }

    public UserCredentials(JObject credentialJson)
    {
        Credentials = credentialJson.ToObject<Dictionary<string, JToken>>()!;
    }

    public JToken Get(string credential)
    {
        return Credentials[credential];
    }
}