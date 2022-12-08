using Microsoft.Extensions.Configuration;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class MainPage : TabbedPage
{
	public MainPage(string authHeder)
	{
        Settings settings = MauiProgram.Services.GetService<IConfiguration>().GetRequiredSection("Settings").Get<Settings>();
        MauiProgram.Hub = new(authHeder, settings.BaseUrl, settings.HubUri);
        InitializeComponent();
        RetriveSheets(settings, authHeder);
    }

    private static async void RetriveSheets(Settings s, string auth)
    {
        string user = await SecureStorage.Default.GetAsync("username");

        if (user is not null)
        {
            RestService<Dictionary<int, MauiPlayer>, MauiPlayer> restService = new(new Uri(s.BaseUrl), auth);
            await restService.RefreshDataAsync(s.RestUriSheet + user);
            MauiProgram.Sheets = restService.ReturnStruct;
        }
    }
}