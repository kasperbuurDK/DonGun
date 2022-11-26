using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using SharedClassLibrary.Actions;
using SharedClassLibrary.MessageStrings;
using System.Linq.Expressions;
using System.Net;

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
        MauiProgram.Hub = null;
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

    // Debug -- Remove
    private async void UpdateBtnClicked(object sender, EventArgs e)
    {
        Settings settings = MauiProgram.Services.GetService<IConfiguration>().GetRequiredSection("Settings").Get<Settings>();
        string authHeder = await SecureStorage.Default.GetAsync("authHeader");
        HubService don = new(authHeder, settings.BaseUrl, settings.HubUri, true);
        await don.Initialise();
        await don.JoinRoom(SKey.Text);
        Queue<Character> myQ = new();
        myQ.Enqueue(new Player() { Signature = "SomeDude1"});
        myQ.Enqueue(new Npc() { Signature = "SomeDude2"});
        List<string> misHaps = new() {"What ", "Is ", "This ", "For ", "? " };
        NewTurnMessage msg = new(SKey.Text, myQ, misHaps);
        await don.Send(msg);
        UpdateMessage Umsg = new()
        {
            ConnectionId = MauiProgram.Hub.ConnectionId,
            UpdateStr = "Hello?",
            possibleActions = new List<IAnAction>()
        };
        Umsg.possibleActions.Add(new OffensiveAction("SomeDude1", "SomeDude2"));
        await don.Send(Umsg);
    }

    private async void JoinBtnClicked(object sender, EventArgs e)
    {
        SelectPopUp popup = new();
        var result = await this.ShowPopupAsync(popup);

        if (result is int IntResult)
        {
            MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => LockJoin(args.Messege));
            try
            {
                await MauiProgram.Hub.Initialise();
            }
            catch (HttpRequestException ex)
            {
                ErrorLabel.Text = ex.Message;
            }
            if (!string.IsNullOrEmpty(SKey.Text))
            {
                MauiProgram.Sheet = MauiProgram.Sheets[IntResult];
                await MauiProgram.Hub.JoinRoom(SKey.Text, MauiProgram.Sheets[IntResult]);
            }
        }
    }

    private void LockJoin(HubServiceException args)
    {
        ErrorLabel.Text = args.Messege;
        if (args.ActionName == "JoinGameRoom" && args.Code == (int)HttpStatusCode.OK)
        {
            SKey.IsEnabled = false;
            Join.IsEnabled = false;
            UserOptions.IsEnabled = false;
        }
        if (args.ActionName == "LeaveGameRoom" && args.Code == (int)HttpStatusCode.OK)
        {
            SKey.IsEnabled = true;
            Join.IsEnabled = true;
            UserOptions.IsEnabled = true ;
        }
    }


    private async void LeaveBtnClicked(object sender, EventArgs e)
    {
        try
        {
            await MauiProgram.Hub.LeaveRoom(SKey.Text);
        }
        catch (HttpRequestException ex)
        {
            ErrorLabel.Text = ex.Message;
        }
    }

    private async void RemoveActBtnClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete acount?", "By removing the acount, all user data, including sheets, will be lost!", "Delete", "Cancel");
        if (answer)
        {
            deleteUserAct.IsEnabled = false;
            Settings settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
            string authHeder = await SecureStorage.Default.GetAsync("authHeader");
            string user = await SecureStorage.Default.GetAsync("username");
            RestService<List<User>, User> RestUserInfo = new(new Uri(settings.BaseUrl), authHeder);
            await RestUserInfo.DeleteDataAsync(MauiProgram.Connectivity, settings.RestUriUser + user);
            MauiProgram.Connectivity = null;
            Application.Current.MainPage = new LoginPage();
            deleteUserAct.IsEnabled = true;
        }
    }

    public void EntryTextChanged(object obj, EventArgs args)
    {
        if (SKey.Text.Length > 0)
            Join.IsEnabled = true;
        else
            Join.IsEnabled = false;
    }
}