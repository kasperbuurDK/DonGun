namespace PlayerSide.Pages;

public partial class CharPage : ContentPage
{
    public CharPage()
    {
        InitializeComponent();
        CharPc.Character = Globals.Connectivity;
        Globals.RService.ResourceChanged += OnConnectivityChanged;
    }

    // Test to see if this is even needed...
    public void OnConnectivityChanged(object sender, EventArgs e)
    {
        CharPc.Character = Globals.Connectivity;
    }
}