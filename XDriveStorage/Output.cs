using CommandDotNet;

namespace XDriveStorage;

public static class Output
{
    public static void WriteInfo(object? text)
    {
        WriteLine(text);
    }
    
    public static void WriteWarning(object? text)
    {
        RunWithForegroundColor(ConsoleColor.Yellow, () => WriteLine(text));
    }
    
    public static void WriteError(object? text)
    {
        RunWithForegroundColor(ConsoleColor.Red, () => WriteLine(text));
    }
    
    public static void WriteLine(object? text)
    {
        Write(text + Environment.NewLine);
    }
    
    public static void Write(object? text)
    {
        if (!Program.Quiet)
            Program.IConsole.Write(text);
    }

    private static void RunWithForegroundColor(ConsoleColor foregroundColor, Action action)
    {
        var originalForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        
        action.Invoke();

        Console.ForegroundColor = originalForegroundColor;
    }
}