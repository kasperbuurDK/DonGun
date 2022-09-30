namespace PlayerSide;

public partial class CharPage : ContentPage
{
    public CharPage()
    {
        InitializeComponent();
        CharPc.Character = Globals.Connectivity;
    }
}