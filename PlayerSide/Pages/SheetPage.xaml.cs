using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    public SheetPage()
    {
        InitializeComponent();
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => ShowView(args.Messege));
        //MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => ShowView(args.Messege));
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

