using Microsoft.Extensions.Configuration;
using SharedClassLibrary.MessageStrings;

namespace PlayerSide.Pages;

public partial class OptionsPage : ContentPage
{
    IConfiguration _configuration;

    public OptionsPage()
    {
        InitializeComponent();
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
    }

    private void LogoutBtnClicked(object sender, EventArgs e)
    {
        // Sent dirty data back to server.
        MauiProgram.Connectivity = null;
        Application.Current.MainPage = new LoginPage();
    }

    private async void SendFileUpdateBtnClicked(object sender, EventArgs e)
    {
        // Sent dirty data back to server.
        try
        {
            await MauiProgram.Hub.Send(new FileUpdateMessage() {UserName = "User", UUID = "´09325ujsr0394", LastModified = DateTime.Now.ToString(), SheetId = "14" });
        }
        catch (ArgumentNullException ex)
        {
            if (ex.ParamName is "SessionKey")
                ErrorLabel.Text = "Player not join a game room yet!";
        }
        catch (NullReferenceException)
        {
            ErrorLabel.Text = "Hub no created!";
        }
    }

    private async void UserOptionsBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserPage());
    }

    private async void Debug2BtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MovePage());
    }

    private async void Debug3BtnClicked(object sender, EventArgs e)
    {
        Settings settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
        string authHeder = await SecureStorage.Default.GetAsync("authHeader");
        MauiProgram.Hub = new(authHeder, settings.BaseUrl, settings.HubUri);
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => ErrorLabel.Text = args.Messege.Messege;
        try
        {
            await MauiProgram.Hub.Initialise();
        }
        catch (HttpRequestException ex)
        {
            ErrorLabel.Text = ex.Message;
        }
        if (!string.IsNullOrEmpty(SKey.Text))
            await MauiProgram.Hub.JoinRoom(SKey.Text);
    }
}