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
        Globals.RestPlayerInfo = null;
        Globals.RestNonPlayerInfo = null;
        Application.Current.MainPage = new LoginPage();
    }

    private async void SendFileUpdateEventBtnClicked(object sender, EventArgs e)
    {
        // Sent dirty data back to server.
        await Globals.FileUpdateHub.Send(new SharedClassLibrary.FileUpdateMessage() {UserName = "User", UUID = "´09325ujsr0394", LastModified = DateTime.Now.ToString(), SheetId = "14" } );
    }
}