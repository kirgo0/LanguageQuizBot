using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DotNetEnv;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using LanguageQuizBot.Modules;
using LanguageQuizBot.Services;
using Telegram.Bot;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureAppConfiguration((context, config) =>
        {
            config.AddJsonFile($"appsettings.json", optional: false);
            config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true);
        })
        .UseSerilog((hostingContext, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
        })
        .ConfigureServices((context, services) =>
        {
            services.AddMemoryCache();

            Env.Load();

            var envVariables = new Dictionary<string, string>
            {
                { "BOT_TOKEN", Environment.GetEnvironmentVariable("BOT_TOKEN") },
                { "BOT_NAME", Environment.GetEnvironmentVariable("BOT_NAME") }
            };

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(envVariables["BOT_TOKEN"]));
            services.AddHostedService<TelegramBotClientBackgroundService>();
        })
        .ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule<HandlersModule>();
        });
    IHost host = builder.Build();
    var hostTask = host.RunAsync();
    await hostTask;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Bot initialization error");
}
finally
{
    Log.CloseAndFlush();
}