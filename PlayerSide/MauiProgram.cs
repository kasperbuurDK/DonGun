namespace PlayerSide;

public static class MauiProgram
{
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

        return builder.Build();
    }
}
