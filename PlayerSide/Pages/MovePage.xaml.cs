using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using SharedClassLibrary;
using SharedClassLibrary.MessageStrings;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using SharedClassLibrary.AuxUtils;

namespace PlayerSide.Pages;

public partial class MovePage : ContentPage
{
    IConfiguration _configuration;
    private ImageButton _selected;
    private static readonly IconTintColorBehavior _selectedBehavior = new()
    {
        TintColor = Application.Current.Resources.MergedDictionaries.First()["Primary"] as Color
    };
    public MovePage()
    {
        InitializeComponent();
        MauiProgram.Hub.UpdateEvent += (object sender, HubEventArgs<UpdateMessage> e) => 
        { 
            UpdateLabel.Text = e.Messege.UpdateStr;
        };
        UpdateFrame.BackgroundColor = Color.FromRgba(255, 255, 255, 128); // Workaround
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
    }

    // Debug -- Remove
    /*
    private async void UpdateBtnClicked(object sender, EventArgs e)
    {
        Settings settings = MauiProgram.Services.GetService<IConfiguration>().GetRequiredSection("Settings").Get<Settings>();
        string authHeder = await SecureStorage.Default.GetAsync("authHeader");
        HubService don = new(authHeder, settings.BaseUrl, settings.HubUri, true);
        don.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => PrintDonResult(don, args.Messege));
        await don.Initialise();
        await don.JoinRoom("kage");
        UpdateMessage msg = new()
        {
            ConnectionId = MauiProgram.Hub.ConnectionId,
            UpdateStr = "Hello?",
        };
        List<IAnAction> PossibleActions = new();
        PossibleActions.Add(new OffensiveAction("SomeDude1", "SomeDude2"));
        msg.PossibleActionsJson = PossibleActions.TypeToJson();
        await don.Send(msg);
    } */

    /*
    // Debug -- Remove
    private async void NewTurnBtnClicked(object sender, EventArgs e)
    {
        Settings settings = MauiProgram.Services.GetService<IConfiguration>().GetRequiredSection("Settings").Get<Settings>();
        string authHeder = await SecureStorage.Default.GetAsync("authHeader");
        HubService don = new(authHeder, settings.BaseUrl, settings.HubUri, true);
        don.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => PrintDonResult(don, args.Messege));
        await don.Initialise();
        await don.JoinRoom("kage");
        Queue<Character> myQ = new();
        myQ.Enqueue(new Player() { Signature = "SomeDude1" });
        myQ.Enqueue(new Npc() { Signature = "SomeDude2" });
        List<string> misHaps = new() { "What ", "Is ", "This ", "For ", "? " };
        NewTurnMessage msg = new("kage", myQ, misHaps);
        await don.Send(msg);
    }*/
    /*
    private void PrintDonResult(HubService don, HubServiceException messege)
    {
        ErrorLabel.Text = messege.Messege;
    }*/

    private async void ArrowBtnClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton btn)
        {
            if (_selected == btn && btn != null)
                return;
            if (btn.ClassId != "5")
            {
                if (_selected is not null)
                    _selected.Behaviors.Remove(_selectedBehavior);
                btn.Behaviors.Add(_selectedBehavior);
                _selected = btn;
            }
            else if (_selected is not null)
            {
                if (MauiProgram.Hub is not null && MauiProgram.Hub.IsConnected) {
                    try
                    {
                        await MauiProgram.Hub.Send(new MoveMessage() { Direction = ((MoveDirections)int.Parse(_selected.ClassId)).ToString(), Distence = (int)_stepper.Value });
                    } catch (Microsoft.AspNetCore.SignalR.HubException ex)
                    {
                        ErrorLabel.Text = ex.Message;
                    }
                    _selected.Behaviors.Remove(_selectedBehavior);
                    _selected = null;
                }else 
                {
                    ErrorLabel.Text = "No alive connection to game server!";
                }
            }    
        }
    }
}