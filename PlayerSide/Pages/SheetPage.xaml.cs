using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private SheetView? _cVChild;

    public SheetPage()
    {
        InitializeComponent();
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => SetCharView(args.Messege));
    }

    public SheetPage(MauiPlayer mPChild) : this()
    {
        _cVChild = new SheetView(mPChild);
        MainGridSheet.Add(_cVChild);
    }

    public void SetCharView(HubServiceException args)
    {
        if (args.ActionName == "JoinGameRoom" && args.Code == (int)System.Net.HttpStatusCode.OK)
        {
            var tabbedpage = Parent as TabbedPage;
            tabbedpage.Children.Remove(this);
            tabbedpage.Children.Insert(0, new SheetPage(MauiProgram.Sheet));
        }
        if (args.ActionName == "LeaveGameRoom" && args.Code == (int)System.Net.HttpStatusCode.OK)
        {
            MainGridSheet.Remove(_cVChild);
        }
    }
}

