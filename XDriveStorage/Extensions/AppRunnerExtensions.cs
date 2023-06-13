using CommandDotNet;
using CommandDotNet.Execution;
using CommandDotNet.IoC.SimpleInjector;

using XDriveStorage.Configuration;
using XDriveStorage.Drives;

namespace XDriveStorage.Extensions;

public static class AppRunnerExtensions
{
    public static AppRunner RegisterSimpleInjector(this AppRunner appRunner)
    {
        var container = new SimpleInjector.Container();
     
        container.RegisterInstance<IAppConfiguration>(new AppConfiguration());
        
        foreach(var (type, subcommandAttr) in appRunner.GetCommandClassTypes())
        {
            container.Register(type);
        }
        
        appRunner.UseSimpleInjector(container);

        return appRunner;
    }
}