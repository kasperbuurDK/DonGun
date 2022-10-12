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
                PageLock(true);
                Globals.RestPlayerInfo = new RestService<Player>(userEntry.Text, passEntry.Text);
                try
                {
                    await Globals.RestPlayerInfo.RefreshDataAsync(Constants.RestUriGet + Globals.RestPlayerInfo.UserName);
                    if (Globals.RestPlayerInfo.Response.IsSuccessStatusCode)
                    {
                        Globals.Connectivity = Globals.RestPlayerInfo.Items[0];
                        Globals.FileUpdateHub = new(Globals.RestPlayerInfo.AuthHeader);
                        await Globals.FileUpdateHub.Initialise();
                        Application.Current.MainPage = new AppShell();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"An error accured - \"{ex.Message}\"";
                }
                if (Globals.RestPlayerInfo.Response is not null && Globals.RestPlayerInfo.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
