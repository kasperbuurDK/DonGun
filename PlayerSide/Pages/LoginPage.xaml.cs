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
				Globals.RService = new RestService<Player>();
				Globals.RService.UserName = userEntry.Text;
				Globals.RService.UserPassword = passEntry.Text;
				await Globals.RService.RefreshDataAsync("/api/weatherforecast/Get/" + Globals.RService.UserName);
				if (Globals.RService.Response.IsSuccessStatusCode)
				{
					Globals.Connectivity = Globals.RService.Items[0];
					Application.Current.MainPage = new AppShell();
				}
				else
				{
					ActivityIndicator.IsRunning = false;
					loginBtn.IsEnabled = true;
                    Globals.RService = null;
                    errorLabel.Text = "Invalid Username or Password!";
				}
			}
        } 
		else 
			errorLabel.Text = "Please input a Username and Password!";

    }
}