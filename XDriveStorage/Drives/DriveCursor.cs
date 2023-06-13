using System.Numerics;

using XDriveStorage.Configuration;

namespace XDriveStorage.Drives;

public class DriveCursor
{
    private IAppConfiguration Configuration { get; }
    
    public IDrive CurrentDrive { get; private set; }
    public BigInteger SeekPosition { get; private set; }

    public DriveCursor(IAppConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void Seek(BigInteger position)
    {
        
    }
}