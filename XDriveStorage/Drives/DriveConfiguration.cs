using Newtonsoft.Json;

namespace XDriveStorage.Drives;

public class DriveConfiguration
{
    public ulong MaxFileSize { get; set; } = 25000000;
}