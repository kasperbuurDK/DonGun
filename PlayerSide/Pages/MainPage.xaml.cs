using Microsoft.Extensions.Configuration;

namespace PlayerSide.Pages;

public partial class MainPage : TabbedPage
{
	public MainPage(string authHeder)
	{
        Settings settings = MauiProgram.Services.GetService<IConfiguration>().GetRequiredSection("Settings").Get<Settings>();
        MauiProgram.Hub = new(authHeder, settings.BaseUrl, settings.HubUri);
        InitializeComponent();
	}
}