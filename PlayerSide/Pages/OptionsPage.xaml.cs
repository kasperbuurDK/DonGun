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
        MauiProgram.Connectivity = null;
        MauiProgram.RestUserInfo = null;
        Application.Current.MainPage = new LoginPage();
    }

    private async void SendFileUpdateBtnClicked(object sender, EventArgs e)
    {
        // Sent dirty data back to server.
        await MauiProgram.FileUpdateHub?.Send(new SharedClassLibrary.FileUpdateMessage() {UserName = "User", UUID = "´09325ujsr0394", LastModified = DateTime.Now.ToString(), SheetId = "14" } );
    }

    private async void UserOptionsBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserPage());
    }

    private void Debug1BtnClicked(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}