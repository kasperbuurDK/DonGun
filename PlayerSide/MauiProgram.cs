using Microsoft.Extensions.Configuration;
using PlayerSide.Pages;
using SharedClassLibrary;
using System.Reflection;

namespace PlayerSide;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }
    public static Character Connectivity { get; set; }
    public static List<Character> GameOrder { get; set; }
    public static RestService<List<User>, User> RestUserInfo { get; set; }
    public static HubService<FileUpdateMessage> FileUpdateHub { get; set; }

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("DalelandsUncial-BOpn", "DalelandsRegular");
                fonts.AddFont("DalelandsUncialBold-82zA.ttf", "Dalelandsbold");
                fonts.AddFont("Iokharic-dqvK", "IokharicRegular");
                fonts.AddFont("IokharicBold-Plor.ttf", "Iokharicbold");
            });
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerSide.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

        builder.Configuration.AddConfiguration(config);

        var app = builder.Build();
        Services = app.Services;

        return app;
    }
}