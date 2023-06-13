using CommandDotNet;
using CommandDotNet.Extensions;
using CommandDotNet.Prompts;

using XDriveStorage.Attributes;
using XDriveStorage.Commands;
using XDriveStorage.Extensions;

namespace XDriveStorage;

[Command("xdrive")]
public class Program
{
    public static bool Verbose { get; private set; }
    public static bool Quiet { get; private set; }
    
    public static int Main(string[] args)
    {
        return new AppRunner<Program>()
            .UsePrompter()
            .RegisterSimpleInjector()
            .UseArgumentPrompter(
                argumentPrompterFactory: (context, prompter) => new ArgumentPrompter(prompter, (ctx, argument) =>  
                    $"{argument.GetCustomAttribute<ArgumentMissingPrompt>()?.PromptText}"),
                argumentFilter: argument => 
                    argument.GetCustomAttribute<ArgumentMissingPrompt>() != null 
                    && argument.Arity.RequiresAtLeastOne() 
                    && !argument.HasValueFromInputOrDefault())
            .Run(args);
    }

    public Task<int> Interceptor(
        InterceptorExecutionDelegate next,
        CommandContext ctx,
        [Option('v', AssignToExecutableSubcommands = true)] bool verbose = false,
        [Option('v', AssignToExecutableSubcommands = true)] bool quiet = false)
    {
        Verbose = verbose;
        Quiet = quiet;
        
        if (quiet)
            Console.SetOut(new StringWriter());
        
        return next();
    }
    
    [Subcommand(RenameAs = "root")]
    public RootCommand RootCommand { get; set; }
    
    [Subcommand(RenameAs = "upload")]
    public UploadCommand UploadCommand { get; set; }
    
    [Subcommand(RenameAs = "download")]
    public DownloadCommand DownloadCommand { get; set; }
    
    [Subcommand(RenameAs = "patch")]
    public PatchCommand PatchCommand { get; set; }
    
    [Subcommand(RenameAs = "services")]
    public DrivesCommand DrivesCommand { get; set; }
    
    [Subcommand(RenameAs = "users")]
    public UsersCommand UsersCommand { get; set; }
}