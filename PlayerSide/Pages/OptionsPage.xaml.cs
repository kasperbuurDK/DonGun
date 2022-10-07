namespace PlayerSide.Pages;

public partial class OptionsPage : ContentPage
{
	public OptionsPage()
	{
		InitializeComponent();
	}

    private void LogoutBtnClicked(object sender, EventArgs e)
    {
        // Sent dirty data back to server.
        Globals.Connectivity = null;
        Globals.RSPlayerInfo = null;
        Globals.RSNonPlayerInfo = null;
        Application.Current.MainPage = new LoginPage();
    }
}