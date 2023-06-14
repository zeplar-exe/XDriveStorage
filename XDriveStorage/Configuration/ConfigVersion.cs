using Newtonsoft.Json;

namespace XDriveStorage.Configuration;

[JsonConverter(typeof(ConfigVersion.Converter))]
public struct ConfigVersion : IEquatable<ConfigVersion>
{
    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }

    public static ConfigVersion Zero => new(0, 0, 0);
    public static ConfigVersion Current => new(0, 0, 0);

    public ConfigVersion(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public bool Equals(ConfigVersion other)
    {
        return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
    }

    public override bool Equals(object? obj)
    {
        return obj is ConfigVersion other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, Patch);
    }

    public static bool operator ==(ConfigVersion left, ConfigVersion right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ConfigVersion left, ConfigVersion right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    private class Converter : JsonConverter<ConfigVersion>
    {
        public override void WriteJson(JsonWriter writer, ConfigVersion value, JsonSerializer serializer)
        {
            writer.WriteToken(JsonToken.String, value.ToString());
        }

        public override ConfigVersion ReadJson(JsonReader reader, Type objectType, ConfigVersion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var str = reader.Value?.ToString();

            if (str == null)
                return ConfigVersion.Zero;

            var parts = str.Split('.');
            var versionNumbers = new int[3];

            for (var i = 0; i < 3; i++)
            {
                if (i < parts.Length)
                {
                    versionNumbers[i] = int.TryParse(parts[i], out var n) ? n : 0;
                }
                else
                {
                    versionNumbers[i] = 0;
                }
            }

            return new ConfigVersion(versionNumbers[0], versionNumbers[1], versionNumbers[2]);
        }
    }
}