using SharedClassLibrary;

namespace PlayerSide;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        Globals.RService = new RestService<Npc>();
        MainPage = new LoginPage();
    }
}
