using Newtonsoft.Json.Linq;

using XDriveStorage.Drives;

namespace XDriveStorage.Users;

public class UserCredentials : Dictionary<string, JToken>
{
    public UserCredentials()
    {
        
    }

    public UserCredentials(JObject json)
    {
        foreach (var (key, value) in json)
        {
            this[key] = value!;
        }
    }
}