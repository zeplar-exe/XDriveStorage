using CommandDotNet;

namespace XDriveStorage;

public static class Output
{
    public static void WriteInfo(object? text, OutputType type = OutputType.Normal)
    {
        WriteLine(text, type);
    }
    
    public static void WriteWarning(object? text, OutputType type = OutputType.Normal)
    {
        RunWithForegroundColor(ConsoleColor.Yellow, () => WriteLine(text, type));
    }
    
    public static void WriteError(object? text, OutputType type = OutputType.Normal)
    {
        RunWithForegroundColor(ConsoleColor.Red, () => WriteLine(text, type));
    }
    
    public static void WriteLine(object? text, OutputType type = OutputType.Normal)
    {
        Write(text + Environment.NewLine, type);
    }
    
    public static void Write(object? text, OutputType type = OutputType.Normal)
    {
        if (Program.Quiet)
            return;

        if (type == OutputType.Verbose && !Program.Verbose)
            return;
        
        Program.IConsole.Write(text);
    }

    private static void RunWithForegroundColor(ConsoleColor foregroundColor, Action action)
    {
        var originalForegroundColor = Console.ForegroundColor;
        Program.IConsole.ForegroundColor = foregroundColor;
        
        action.Invoke();

        Program.IConsole.ForegroundColor = originalForegroundColor;
    }
}

public enum OutputType
{
    Normal,
    Verbose
}