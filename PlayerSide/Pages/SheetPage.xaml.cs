using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using SharedClassLibrary.Actions;
using SharedClassLibrary.MessageStrings;
using System.Collections.Generic;
using System.Linq;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private Border priSelected = new();

    public SheetPage()
    {
        InitializeComponent();
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => ShowView(args.Messege));
        MauiProgram.Hub.UpdateEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => UpdateActionList(args.Messege));
    }

    private void UpdateActionList(UpdateMessage messege)
    {
        SheetActionStackLayout.Clear();
        if (messege.PossibleActionsJson is null || messege.PossibleActionsJson.JsonToType<List<AnAction>>().Count == 0)
            return;
        foreach (AnAction p in messege.PossibleActionsJson.JsonToType<List<AnAction>>())
        {
            Grid grid = new()
            {
                new AnActionView(p)
            };
            Button button = new() { Opacity = 0, Padding = 1 };
            Border border = new()
            {
                Stroke = Color.Parse("Transparent"),
                Background = Color.Parse("Transparent"),
                StrokeThickness = 5,
                Margin = new Thickness(1.5, 1.5, 1.5, 1.5),
                Content = button
            };
            button.Clicked += (sender, args) => MainThread.BeginInvokeOnMainThread(async () => 
            {
                if (!border.Equals(priSelected))
                {
                    border.Stroke = Application.Current.Resources.MergedDictionaries.First()["Primary"] as Color;
                    priSelected.Stroke = Color.Parse("Transparent");
                    priSelected = border;
                    // When double clcked, perfom popup for roll, and send.
                } else
                {
                    // D20 Default for now.
                    DicePopUp popup = new("dtwenty", 1, 20);
                    var result = await this.ShowPopupAsync(popup);
                    if (result is not null)
                    {
                        await MauiProgram.Hub.Send(new ActionMessage(MauiProgram.Hub.SessionKey, p.Signature, (int)result));
                        SheetActionStackLayout.Clear();
                    }
                }
            });
            grid.Add(border);
            SheetActionStackLayout.Add(grid);
        }        
    }

    private void ShowView(HubServiceException args)
    {
        if (args.ActionName == "JoinGameRoom" && args.Code == (int)System.Net.HttpStatusCode.OK)
        {
            MainGrid.Opacity = 1;
            if (string.IsNullOrEmpty(MauiProgram.Sheet.ImageName))
                MauiProgram.Sheet.ImageName = "no_data.png";
            CharaImg.Source = MauiProgram.Sheet.ImageName;
            NameText.Text = MauiProgram.Sheet.Name;
            IconDex.Text = MauiProgram.Sheet.Dexterity.ToString();
            IconStr.Text = MauiProgram.Sheet.Strength.ToString();
            IconWis.Text = MauiProgram.Sheet.Wisdome.ToString();
            IconInt.Text = MauiProgram.Sheet.Intelligence.ToString();
            IconCon.Text = MauiProgram.Sheet.Constitution.ToString();
            IconCha.Text = MauiProgram.Sheet.Charisma.ToString();
        }
        if (args.ActionName == "LeaveGameRoom" && args.Code == (int)System.Net.HttpStatusCode.OK)
        {
            MainGrid.Opacity = 0;
        }
    }
}

