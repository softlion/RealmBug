using Microsoft.Extensions.Logging;

namespace RealmBug20240322;

public static partial class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        AppDomain.CurrentDomain.UnhandledException += HandleUnhandledException;
        TaskScheduler.UnobservedTaskException += HandleUnobservedTaskException;
        
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    
    #region global exception reporting

    private static void Error(string message)
    {
        Console.WriteLine(message);
    }

    private static void HandleUnhandledException(object s, UnhandledExceptionEventArgs args)
    {
        string? message;
        if (args.ExceptionObject is Exception ex)
            message = ex.Message;
        else
            message = args?.ToString();

        Error($"UnhandledException from {s.GetType()}: {message}");

        if (global::System.Diagnostics.Debugger.IsAttached)
            global::System.Diagnostics.Debugger.Break();
    }

    private static void HandleUnobservedTaskException(object? s, UnobservedTaskExceptionEventArgs e)
    {
        Error($"UnobservedTaskException from {s?.GetType()}: {e.Exception.Message}");
        var i = 1;
        foreach (var exception in e.Exception.InnerExceptions)
            Error($"UnobservedTaskException innerException #{i++} {exception.Message}");
        if (!e.Observed)
            e.SetObserved();

#if DEBUG
        if (global::System.Diagnostics.Debugger.IsAttached)
            global::System.Diagnostics.Debugger.Break();
#endif
    }

    #endregion
}

