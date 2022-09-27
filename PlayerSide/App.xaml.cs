using SharedClassLibrary;

namespace PlayerSide;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Globals.RService = new RestService<Character_abstract>();
        MainPage = new LoginPage();
    }
}
