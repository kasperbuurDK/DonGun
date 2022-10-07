using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class CharPage : ContentPage
{
    public CharPage()
    {
        InitializeComponent();
        CharPc.Character = Globals.Connectivity;
        Globals.RSNonPlayerInfo = new RestService<Npc>(Globals.RSPlayerInfo.AuthHeader)
        {
            UserName = Globals.RSPlayerInfo.UserName
        };
        Globals.RSPlayerInfo.ResourceChanged += OnConnectivityChanged;
        Globals.RSNonPlayerInfo.ResourceChanged += OnGameOrderChanged;
    }

    private void OnGameOrderChanged(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    // Test to see if this is even needed...
    public void OnConnectivityChanged(object sender, EventArgs e)
    {
        CharPc.Character = Globals.Connectivity;
    }
}