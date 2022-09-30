using DevExpress.Xpo.DB;

namespace PlayerSide;

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
            Globals.RService.UserName = userEntry.Text;
			Globals.RService.UserPassword = passEntry.Text;
			await Globals.RService.RefreshDataAsync("/api/weatherforecast/Get/" + Globals.RService.UserName);
			if (Globals.RService.Response.IsSuccessStatusCode)
			{
				Globals.Connectivity = Globals.RService.Items[0];
				Application.Current.MainPage = new AppShell();
			}
			else
				errorLabel.Text = "Invalid Username or Password!";
        }

    }
}