namespace PlayerSide;

public partial class CharPage : ContentPage
{
    public CharPage()
    {
        InitializeComponent();
        CharPc.CharHp = Globals.Connectivity.HealthCurrent;
        CharPc.CharRes = Globals.Connectivity.ResourceCurrent;
    }
}