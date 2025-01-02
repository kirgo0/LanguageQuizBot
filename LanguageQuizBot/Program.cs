using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Serilog;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using LanguageQuizBot.Modules;
using LanguageQuizBot.Services;
using Telegram.Bot;
using LanguageQuizBot;

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
                { "BOT_NAME", Environment.GetEnvironmentVariable("BOT_NAME") },
                { "MYSQL_CONNECTION", Environment.GetEnvironmentVariable("MYSQL_CONNECTION") }
            };

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    envVariables["MYSQL_CONNECTION"],
                    ServerVersion.AutoDetect(envVariables["MYSQL_CONNECTION"])
                ));

            services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(envVariables["BOT_TOKEN"]));
            services.AddHostedService<TelegramBotClientBackgroundService>();
        })
        .ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule<HandlersModule>();
        });

    IHost host = builder.Build();

    // Apply migrations
    using (var scope = host.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }

    var hostTask = host.RunAsync();
    await hostTask;
}
catch (Exception ex)
{
    if(ex is not HostAbortedException)
    Log.Fatal(ex, "Bot initialization error");
}
finally
{
    Log.CloseAndFlush();
}
