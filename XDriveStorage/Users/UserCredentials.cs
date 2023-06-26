using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XDriveStorage.Users;

[JsonConverter(typeof(PrivateConverter))]
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

    private class PrivateConverter : JsonConverter<UserCredentials>
    {
        public override void WriteJson(JsonWriter writer, UserCredentials? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
            }
            else
            {
                value.Json.WriteTo(writer);
            }
        }

        public override UserCredentials? ReadJson(JsonReader reader, Type objectType, UserCredentials? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new UserCredentials(JObject.Load(reader));
        }
    }
}