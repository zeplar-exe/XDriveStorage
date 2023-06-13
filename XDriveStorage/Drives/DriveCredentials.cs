using System.Collections;

namespace XDriveStorage.Drives;

public class DriveCredentials : IEnumerable<string>
{
    private string[] Credentials { get; }
    
    public DriveCredentials(string[] credentials)
    {
        Credentials = credentials;
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var credential in Credentials)
            yield return credential;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}