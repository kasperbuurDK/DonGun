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
                RestService<List<User>, User> RestUserInfo = new(new Uri(settings.BaseUrl), userEntry.Text, passEntry.Text);
                try
                {
                    await RestUserInfo.RefreshDataAsync(settings.RestUriUser + RestUserInfo.UserName);
                    if (RestUserInfo.Response.IsSuccessStatusCode)
                    {
                        await SecureStorage.Default.SetAsync("authHeader", RestUserInfo.AuthHeader);
                        await SecureStorage.Default.SetAsync("username", RestUserInfo.UserName);
                        Application.Current.MainPage = new MainPage(RestUserInfo.AuthHeader);
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"An error accured - Logger: \"{RestUserInfo.Logger}\"\n Ex: \"{ex.Message}\"";
                }
                if (RestUserInfo.Response is not null && RestUserInfo.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    errorLabel.Text = "Invalid Username or Password!";
                PageLock(false);
			}
        } 
		else 
			errorLabel.Text = "Please input a Username and Password!";
    }

    private async void CreateBtnClicked(object sender, EventArgs e)
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
                RestService<List<User>, User> RestUserInfo = new RestService<List<User>, User>(new Uri(settings.BaseUrl), userEntry.Text, passEntry.Text);
                try
                {
                    await RestUserInfo.SaveDataAsync(new User() { Name = userEntry.Text, Password = passEntry.Text }, settings.RestUriUser + userEntry.Text, true);
                    if (RestUserInfo.Response.IsSuccessStatusCode)
                    {
                        await SecureStorage.Default.SetAsync("authHeader", RestUserInfo.AuthHeader);
                        await SecureStorage.Default.SetAsync("username", RestUserInfo.UserName);
                        Application.Current.MainPage = new MainPage(RestUserInfo.AuthHeader);
                    }
                    if (RestUserInfo.Response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        errorLabel.Text = $"Usern with this username \"{RestUserInfo.UserName}\" already exsist!";
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"An error accured - \"{ex.Message}\"";
                }
                if (RestUserInfo.Response is not null && RestUserInfo.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
