using SharedClassLibrary;

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
				Globals.RSPlayerInfo = new RestService<Player>(userEntry.Text, passEntry.Text);
                if (await GetCharaFromServer())
				{
					Globals.RSPlayerInfo.CallBackRefreshFunc = GetCharaFromServer;
					Application.Current.MainPage = new AppShell();
				}
				else
				{
					ActivityIndicator.IsRunning = false;
					loginBtn.IsEnabled = true;
                    Globals.RSPlayerInfo = null;
                    errorLabel.Text = "Invalid Username or Password!";
				}
			}
        } 
		else 
			errorLabel.Text = "Please input a Username and Password!";
    }

	private static async Task<bool> GetCharaFromServer()
	{
        await Globals.RSPlayerInfo.RefreshDataAsync(Constants.RestUriGet + Globals.RSPlayerInfo.UserName);
        if (Globals.RSPlayerInfo.Response.IsSuccessStatusCode)
        {
            Globals.Connectivity = Globals.RSPlayerInfo.Items[0];
			return true;
        }
		return false;
    }
}