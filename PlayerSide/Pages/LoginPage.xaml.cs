using SharedClassLibrary;
using System.Reflection.Metadata;

namespace PlayerSide.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private async void LoginBtnClicked(object sender, EventArgs e)
	{
		if (userEntry.Text is not null && passEntry.Text is not null)
		{
			if (userEntry.Text == String.Empty || passEntry.Text == String.Empty)
			{
				errorLabel.Text = "Please input a Username and Password!";
			}
			else
			{
				ActivityIndicator.IsRunning = true;
				loginBtn.IsEnabled = false;
				Globals.RService = new RestService<Player>(userEntry.Text, passEntry.Text);
				if (await GetCharaFromServer())
				{
					Globals.RService.CallBackRefreshFunc = GetCharaFromServer;
					Application.Current.MainPage = new AppShell();
				}
				else
				{
					ActivityIndicator.IsRunning = false;
					loginBtn.IsEnabled = true;
                    Globals.RService = null;
				}
			}
        } 
		else 
			errorLabel.Text = "Please input a Username and Password!";

    }

	private async Task<bool> GetCharaFromServer()
	{
		try
		{
			await Globals.RService.RefreshDataAsync(Constants.RestUriGet + Globals.RService.UserName);
			if (Globals.RService.Response.IsSuccessStatusCode)
			{
				Globals.Connectivity = Globals.RService.Items[0];
				return true;
			}
		} catch (Exception ex)
		{
            errorLabel.Text = $"An error accured - \"{ex.Message}\"";
        }
        if (Globals.RService.Response is not null && Globals.RService.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            errorLabel.Text = "Invalid Username or Password!";
        return false;
    }
}