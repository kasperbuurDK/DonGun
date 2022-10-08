using PlayerSide.Models;

namespace PlayerSide.Pages;

public partial class MainPage : ContentPage
{
    public TestViewModel ViewModel { get; set; }
    public MainPage()
    {
        InitializeComponent();
        DataInnit();

        // HUB:
        //Globals.RestPlayerInfo.ResourceChanged += OnMainCharaUpdateEvent;
        ViewModel = new TestViewModel();
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.Initialise();
    }

    // Test to see if this is even needed...
    public void OnMainCharaUpdateEvent(object sender, EventArgs e)
    { 
        DataInnit();
    }

    private void DataInnit()
    {
        // NameText.Text = (Player)Globals.Connectivity.Name;
        // CharaImg.Source = Globals.Connectivity.Img;
        IconDex.Text = Globals.Connectivity.Dexterity.ToString();
        IconStr.Text = Globals.Connectivity.Strength.ToString();
        IconWis.Text = Globals.Connectivity.Wisdome.ToString();
        IconInt.Text = Globals.Connectivity.Intelligence.ToString();
        IconCon.Text = Globals.Connectivity.Constitution.ToString();
        IconCha.Text = Globals.Connectivity.Charisma.ToString();
    }
}

