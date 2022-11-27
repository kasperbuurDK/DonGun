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
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => ThrowPopUp(args.Messege));
        RetriveSheets(settings, authHeder);
    }

    private async void ThrowPopUp(HubServiceException args)
    {
        if (args.Code != (int)System.Net.HttpStatusCode.OK)
        {
            await DisplayAlert("Exception:", $"{args.Messege}", "Close");
        }
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