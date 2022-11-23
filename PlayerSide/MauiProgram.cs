using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using SharedClassLibrary;
using System.Reflection;

namespace PlayerSide;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; }
    public static User Connectivity { get; set; }
    public static List<Character> GameOrder { get; set; }
    public static Dictionary<int, MauiPlayer> Sheets { get; set; }
    public static HubService Hub { get; set; }

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

        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlayerSide.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

        builder.Configuration.AddConfiguration(config);

        Microsoft.Maui.Handlers.TabbedViewHandler.Mapper.AppendToMapping("FixMultiTab", (handler, view) =>
        {
#if ANDROID
            var viewPager = (AndroidX.ViewPager2.Widget.ViewPager2)handler.PlatformView;
            viewPager.OffscreenPageLimit = 5;
#endif
        });

        var app = builder.Build();
        Services = app.Services;

        return app;
    }
}