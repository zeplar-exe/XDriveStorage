namespace XDriveStorage.Drives;

public record DriveInfo(
    long StorageRemaining, 
    StorageLimit StorageLimit);