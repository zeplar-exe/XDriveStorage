using System.Collections;

using XDriveStorage.Drives;
using XDriveStorage.Drives.Builtin;

namespace XDriveStorage.Configuration;

public class DriveContainer : IEnumerable<IDrive>
{
    private Dictionary<string, IDrive> Drives { get; }

    public DriveContainer()
    {
        Drives = new Dictionary<string, IDrive>();
    }

    public bool Add(IDrive drive)
    {
        if (Drives.ContainsKey(drive.Name))
            return false;
        
        Drives.Add(drive.Name, drive);

        return true;
    }

    public bool TryGet(string name, out IDrive drive)
    {
        return Drives.TryGetValue(name, out drive);
    }

    public bool Exists(string name)
    {
        return Drives.ContainsKey(name);
    }

    public bool Remove(string name)
    {
        return Drives.Remove(name);
    }

    public IEnumerator<IDrive> GetEnumerator()
    {
        return Drives.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}