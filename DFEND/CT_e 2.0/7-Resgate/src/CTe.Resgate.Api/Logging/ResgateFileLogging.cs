using Serilog;
using Serilog.Events;

namespace CTe.Resgate.Api.Logging;

/// <summary>
/// Log de erros/operacional do Resgate em pasta padrão do clone CT_e 2.0.
/// </summary>
public static class ResgateFileLogging
{
    public const string DefaultDirectory =
        @"C:\Users\wmoliveira\Desktop\Clones\CT_e 2.0\Logs";

    public static string ResolveLogDirectory(IConfiguration configuration)
    {
        var configured = configuration["Logging:File:Directory"];
        var dir = string.IsNullOrWhiteSpace(configured)
            ? DefaultDirectory
            : configured.Trim();
        Directory.CreateDirectory(dir);
        return dir;
    }

    public static void Configure(WebApplicationBuilder builder)
    {
        var logDir = ResolveLogDirectory(builder.Configuration);
        Directory.CreateDirectory(logDir);

        var filePath = Path.Combine(logDir, "resgate-.log");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("App", "CTe.Resgate.Api")
            .WriteTo.Console()
            .WriteTo.File(
                filePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                shared: true,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Host.UseSerilog();
        Log.Information("Resgate file logging → {LogDirectory}", logDir);
    }
}
