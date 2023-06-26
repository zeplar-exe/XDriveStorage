using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XDriveStorage.Drives;
using XDriveStorage.Drives.Builtin;
using XDriveStorage.Users;

namespace XDriveStorage.Configuration;

public class AppConfiguration : IAppConfiguration
{
    private string AppDataDirectory => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "xdrive");
    
    [JsonProperty("config_version", Required = Required.Always)] public ConfigVersion Version { get; set; }
    [JsonProperty("root_user_id")] public string? RootUserId { get; set; }
    [JsonProperty("users")] [JsonConverter(typeof(UsersConverter))] public UserContainer Users { get; private set; }
    [JsonIgnore] public DriveContainer Drives { get; } // Custom drives cannot be added at the moment, all are hard-coded
    [JsonProperty("persistent")] private Dictionary<string, JToken> PersistentJson { get; set; }
    

    public AppConfiguration()
    {
        var configFilePath = Path.Join(AppDataDirectory, "configuration.json");
        
        Drives = new DriveContainer
        {
            new GoogleDrive(new DriveConfiguration())
        };

        if (File.Exists(configFilePath))
        {
            using var reader = new JsonTextReader(new StreamReader(configFilePath));
            var json = JObject.Load(reader);
            var serializer = new JsonSerializer();

            serializer.Populate(new JTokenReader(json), this);

            if (json.TryGetValue("drive_builtin", out var builtinDriveJson))
            {
                if (builtinDriveJson is JArray array)
                {
                    foreach (var item in array)
                    {
                        var name = item["name"]!.Value<string>()!;

                        if (Drives.TryGet(name, out var drive))
                        {
                            serializer.Populate(new JTokenReader(item["configuration"]!), drive.Configuration);
                        }
                    }
                }
            }
        }
        else
        {
            Version = ConfigVersion.Current;
            RootUserId = null;
            Users = new UserContainer();
            PersistentJson = new Dictionary<string, JToken>();
        }
    }
    
    public JToken? GetPersistent(string bucket, string name)
    {
        return PersistentJson[bucket]?[name];
    }

    public void StorePersistent(string bucket, string name, JToken value)
    {
        PersistentJson[bucket] ??= new JObject();
        PersistentJson[bucket]![name] = value;
    }

    public void Save()
    {
        var configFilePath = Path.Join(AppDataDirectory, "configuration.json");

        if (!File.Exists(configFilePath))
        {
            Directory.CreateDirectory(AppDataDirectory);
            File.Create(configFilePath).Dispose();
        }
        
        var json = JsonConvert.SerializeObject(this);
        
        File.WriteAllText(configFilePath, json);
    }
    
    private class UsersConverter : JsonConverter<UserContainer>
    {
        public override void WriteJson(JsonWriter writer, UserContainer? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteToken(JsonToken.Null);
                
                return;
            }

            serializer.Serialize(writer, value.ToArray());
        }

        public override UserContainer ReadJson(JsonReader reader, Type objectType, UserContainer? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var users = serializer.Deserialize<User[]>(reader)!;
            var container = existingValue ?? new UserContainer();

            foreach (var user in users)
            {
                container.Add(user);
            }

            return container;
        }
    }
}