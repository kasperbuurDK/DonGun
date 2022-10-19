using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class OrderPage : ContentPage
{
    public OrderPage()
    {
        InitializeComponent();
        /*Globals.RestNonPlayerInfo = new RestService<Npc>(Globals.RestPlayerInfo.AuthHeader)
        {
            UserName = Globals.RestPlayerInfo.UserName
        };*/
        // REST:
        //Globals.RestPlayerInfo.ResourceChanged += OnConnectivityChanged;
        //Globals.RestNonPlayerInfo.ResourceChanged += OnGameOrderChanged;
    }

    private void OnGameOrderChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    // Test to see if this is even needed...
    /*public void OnConnectivityChanged(object sender, EventArgs e)
    {
        CharPc.Character = Globals.Connectivity;
    }*/
}