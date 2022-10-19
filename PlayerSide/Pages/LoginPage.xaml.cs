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
			if (userEntry.Text == string.Empty || passEntry.Text == string.Empty)
			{
				errorLabel.Text = "Please input a Username and Password!";
			}
			else
			{
                PageLock(true);
                Globals.RestUserInfo = new RestService<List<User>, User>(userEntry.Text, passEntry.Text);
                try
                {
                    await Globals.RestUserInfo.RefreshDataAsync(Constants.RestUriUser + Globals.RestUserInfo.UserName);
                    if (Globals.RestUserInfo.Response.IsSuccessStatusCode)
                    {
                        Application.Current.MainPage = new MainPage();
                    }
                }
                catch (Exception ex)
                {
                    errorLabel.Text = $"An error accured - \"{ex.Message}\"";
                }
                if (Globals.RestUserInfo.Response is not null && Globals.RestUserInfo.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
