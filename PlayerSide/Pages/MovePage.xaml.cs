using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using SharedClassLibrary;
using SharedClassLibrary.MessageStrings;
using System.Windows.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace PlayerSide.Pages;

public partial class MovePage : ContentPage
{
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
        UpdateLabel.Text = "Done";
    }

    enum DirectionMapping
    {
        Up = 1,
        Right,
        Down,
        Left
    };

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
                        await MauiProgram.Hub.Send(new MoveMessage() { Direction = ((DirectionMapping)int.Parse(_selected.ClassId)).ToString(), Distence = (int)_stepper.Value });
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