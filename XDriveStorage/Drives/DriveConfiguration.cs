using Newtonsoft.Json;

namespace XDriveStorage.Drives;

public class DriveConfiguration
{
    [JsonProperty("max_file_size")] public ulong MaxFileSize { get; set; } = 25000000;
}