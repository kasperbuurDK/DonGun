using PlayerSide.Views;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class OrderPage : ContentPage
{
    private MauiPlayer timedPlayer;
    Random rnd = new Random();
    Timer _timer1, _timer2;

    public OrderPage()
    {
        InitializeComponent();
        timedPlayer = new() { Name = "hello", HealthMax = 10, HealthCurrent = 5, ResourceMax = 20, ResourceCurrent = 15 };
        MainGrid.Add(new CharView(timedPlayer));
        _timer1 = new Timer(TimerCallBack1, timedPlayer, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        _timer2 = new Timer(TimerCallBack2, timedPlayer, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    private void TimerCallBack1(object state)
    {
        //timedPlayer.HealthCurrent = rnd.Next(0, timedPlayer.HealthMax); 
    }

    private void TimerCallBack2(object state)
    {
        //timedPlayer.ResourceCurrent = rnd.Next(0, timedPlayer.ResourceMax);
    }
}