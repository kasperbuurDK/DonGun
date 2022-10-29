using PlayerSide.Pages;
using SharedClassLibrary;

namespace PlayerSide;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new LoginPage();
    }
}
