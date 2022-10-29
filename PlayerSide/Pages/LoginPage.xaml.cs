using Microsoft.Extensions.Configuration;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class LoginPage : ContentPage
{
    IConfiguration _configuration;
    public LoginPage()
	{
		InitializeComponent();
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
    }

    private async void LoginBtnClicked(object sender, EventArgs e)
	{
		if (userEntry.Text is not null && passEntry.Text is not null)
		{
			if (userEntry.Text == string.Empty || passEntry.Text == string.Empty)
			{
				errorLabel.Text = "Please input a Username and Password!";
			}
			else
			{
                PageLock(true);
                Settings settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
                MauiProgram.RestUserInfo = new RestService<List<User>, User>(new Uri(settings.BaseUrl), userEntry.Text, passEntry.Text);
                try
                {
                    await MauiProgram.RestUserInfo.RefreshDataAsync(settings.RestUriUser + MauiProgram.RestUserInfo.UserName);
                    if (MauiProgram.RestUserInfo.Response.IsSuccessStatusCode)
                    {
                        Application.Current.MainPage = new MainPage();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"An error accured - \"{ex.Message}\"";
                }
                if (MauiProgram.RestUserInfo.Response is not null && MauiProgram.RestUserInfo.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    errorLabel.Text = "Invalid Username or Password!";
                PageLock(false);
			}
        } 
		else 
			errorLabel.Text = "Please input a Username and Password!";
    }

    private void PageLock(bool l)
    {
        ActivityIndicator.IsRunning = l;
        loginBtn.IsEnabled = !l;
    }
}
