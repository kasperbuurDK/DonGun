using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        DataInnit();
        Globals.FileUpdateHub.PropertyChangedEvent += OnUpdateEvent;
    }

    // Test to see if this is even needed...
    public void OnUpdateEvent(object sender, HubEventArgs<FileUpdateMessage> e)
    {
        // Update connectivity here before updating visuals
        DebugLabel.Text = e.Messege.ToString();
        DataInnit();
    }

    private void DataInnit()
    {
        Random rnd = new();
        // NameText.Text = (Player)Globals.Connectivity.Name;
        // CharaImg.Source = Globals.Connectivity.Img;
        IconDex.Text = Globals.Connectivity.Dexterity.ToString();
        IconStr.Text = Globals.Connectivity.Strength.ToString();
        IconWis.Text = Globals.Connectivity.Wisdome.ToString() + rnd.Next(1,50).ToString();
        IconInt.Text = Globals.Connectivity.Intelligence.ToString();
        IconCon.Text = Globals.Connectivity.Constitution.ToString();
        IconCha.Text = Globals.Connectivity.Charisma.ToString();
    }
}

