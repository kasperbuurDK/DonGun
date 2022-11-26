using PlayerSide.Views;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class OrderPage : ContentPage
{
    public Queue<Character> Queue{ get; set; }
    private CharView _cVChild;
    public OrderPage()
    {
        InitializeComponent();
        MauiProgram.Hub.NewTurnEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => RefreshQueue(args.Messege.TheQueue));
        MauiProgram.Hub.ExceptionHandlerEvent += (sender, args) => MainThread.BeginInvokeOnMainThread(() => SetCharView(args.Messege));
    }

    public void RefreshQueue(Queue<Character> q)
    {
        Queue = q;
        MauiProgram.GameOrder = q;
        QueueStackLayout.Clear();
        foreach (Character p in Queue)
        {
            Grid grid = new()
                {
                    new CharView((MauiPlayer)p)
                };
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