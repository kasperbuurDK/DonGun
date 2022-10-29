using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class OrderPage : ContentPage
{
    public OrderPage()
    {
        InitializeComponent();
        /*MauiProgram.RestNonPlayerInfo = new RestService<Npc>(MauiProgram.RestPlayerInfo.AuthHeader)
        {
            UserName = MauiProgram.RestPlayerInfo.UserName
        };*/
        // REST:
        //MauiProgram.RestPlayerInfo.ResourceChanged += OnConnectivityChanged;
        //MauiProgram.RestNonPlayerInfo.ResourceChanged += OnGameOrderChanged;
    }

    private void OnGameOrderChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    // Test to see if this is even needed...
    /*public void OnConnectivityChanged(object sender, EventArgs e)
    {
        CharPc.Character = MauiProgram.Connectivity;
    }*/
}