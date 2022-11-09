using Microsoft.Extensions.Configuration;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class MainPage : TabbedPage
{
	public MainPage(string authHeder)
	{
        Settings settings = MauiProgram.Services.GetService<IConfiguration>().GetRequiredSection("Settings").Get<Settings>();
        MauiProgram.Hub = new(authHeder, settings.BaseUrl, settings.HubUri);
        MauiProgram.Hub.ExceptionHandlerEvent += async (object sender, HubEventArgs<HubServiceException> e) => 
        {
            await DisplayAlert("Exception!", $"{e.Messege.Messege}", "Close");
        };
        InitializeComponent();
	}
}