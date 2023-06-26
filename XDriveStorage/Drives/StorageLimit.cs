using Vogen;

namespace XDriveStorage.Drives;

[ValueObject<long>]
[Instance("Unlimited", -1)]
public partial struct StorageLimit
{
    private static Validation Validate(long input)
    {
        return input >= 0 ? Validation.Ok : Validation.Invalid("Storage limit cannot be less than 0.");
    }
}