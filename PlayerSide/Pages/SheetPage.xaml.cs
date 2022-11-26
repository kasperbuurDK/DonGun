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
    private IAnAction _selected;
    private Border priSelected = new();

    public SheetPage()
    {
        InitializeComponent();
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => ShowView(args.Messege));
        MauiProgram.Hub.UpdateEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => UpdateActionList(args.Messege));
    }

    private void UpdateActionList(UpdateMessage messege)
    {
        SheetStackLayout.Clear();
        _selected = null;
        foreach (IAnAction p in messege.possibleActions)
        {
            Grid grid = new()
            {

                //new AnActionView(p) // Need IanAction view
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
            button.Clicked += delegate (object sender, EventArgs e)
            {
                if (!border.Equals(priSelected))
                {
                    border.Stroke = Application.Current.Resources.MergedDictionaries.First()["Primary"] as Color;
                    _selected = grid.Children.First() as IAnAction;
                    priSelected.Stroke = Color.Parse("Transparent");
                    priSelected = border;
                    // When double clcked, perfom popup for roll, and send.
                } else
                {

                }
            };
            grid.Add(border);
            SheetStackLayout.Add(grid);
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

