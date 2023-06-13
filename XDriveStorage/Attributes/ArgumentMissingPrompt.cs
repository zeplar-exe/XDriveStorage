namespace XDriveStorage.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class ArgumentMissingPrompt : Attribute
{
    public string PromptText { get; }

    public ArgumentMissingPrompt(string promptText)
    {
        PromptText = promptText;
    }
}