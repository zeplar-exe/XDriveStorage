using Newtonsoft.Json.Linq;

using XDriveStorage.Drives;

namespace XDriveStorage.Users;

public class UserCredentials
{
    private JObject Json { get; }

    public UserCredentials(JObject json)
    {
        Json = json;
    }

    public string Get(string key)
    {
        if (!Json.TryGetValue(key, out var value))
            Output.WriteError($"Expected user credential is not present: {key}");

        return value?.Value<string?>() ?? "";
    }
}