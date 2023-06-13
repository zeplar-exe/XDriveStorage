using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using XDriveStorage.Drives;
using XDriveStorage.Drives.Builtin;

namespace XDriveStorage.Configuration;

public class AppConfiguration : IAppConfiguration
{
    private string AppDataDirectory => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "xdrive");
    
    [JsonProperty("config_version")] public ConfigVersion Version { get; set; }
    [JsonProperty("root_user_id")] public string? RootUserId { get; set; }
    [JsonProperty("users")] public UserContainer Users { get; }
    [JsonProperty("drives")] [JsonConverter(typeof(DrivesConverter))] public DriveContainer Drives { get; }
    
    [JsonProperty("persistent")] private JObject PersistentJson { get; }

    public AppConfiguration()
    {
        var configFilePath = Path.Join(AppDataDirectory, "configuration.json");

        Version = ConfigVersion.Zero;
        RootUserId = null;
        Users = new UserContainer();
        Drives = new DriveContainer
        {
            new GoogleDrive(new DriveConfiguration())
        };
        PersistentJson = new JObject();
        
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

    public async void Save()
    {
        var configFilePath = Path.Join(AppDataDirectory, "configuration.json");
        var writer = new JsonTextWriter(new StreamWriter(configFilePath));

        await JObject.FromObject(this).WriteToAsync(writer);
    }

    private class DrivesConverter : JsonConverter<DriveContainer>
    {
        public override void WriteJson(JsonWriter writer, DriveContainer? value, JsonSerializer serializer)
        {
            // Do nothing
        }

        public override DriveContainer? ReadJson(JsonReader reader, Type objectType, DriveContainer? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return existingValue; // Do nothing
        }
    }
}