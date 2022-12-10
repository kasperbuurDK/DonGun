using PlayerSide.Views;
using SharedClassLibrary;
using SharedClassLibrary.Actions;
using SharedClassLibrary.MessageStrings;

namespace PlayerSide.Pages;

public partial class OrderPage : ContentPage
{
    private CharView _cVChild;

    public OrderPage()
    {
        InitializeComponent();
        MauiProgram.Hub.NewTurnEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => RefreshQueue(args.Messege.TheQueue.Decompress().JsonToType<Queue<Character>>()));
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => SetCharView(args.Messege));
    }

    public void RefreshQueue(Queue<Character> q)
    {
        QueueStackLayout.Clear();
        MauiProgram.GameOrder = q.CopyObject();
        foreach (Character p in q)
        {
            if (MauiProgram.Sheet.Signature == p.Signature)
            {
                MauiProgram.Sheet.HealthMax = p.HealthMax;
                MauiProgram.Sheet.HealthCurrent = p.HealthCurrent;
                MauiProgram.Sheet.ResourceMax = p.ResourceMax;
                MauiProgram.Sheet.ResourceCurrent = p.ResourceCurrent;
            }
            Grid grid;
            if (p is MauiPlayer player)
            {
                grid = new()
                {
                    new CharView(player)
                };
            } else
            {
                grid = new()
                {
                    new CharView(p)
                };
            }
            QueueStackLayout.Add(grid);
        }
    }

    public void SetCharView(HubServiceException args)
    {
        if (args.ActionName == "JoinGameRoom" && args.Code == (int)System.Net.HttpStatusCode.OK)
        {
            _cVChild = new(MauiProgram.Sheet);
            MainGrid.Add(_cVChild);
        }
        if (args.ActionName == "LeaveGameRoom" && args.Code == (int)System.Net.HttpStatusCode.OK)
        {
            MainGrid.Remove(_cVChild);
        }
    }
}