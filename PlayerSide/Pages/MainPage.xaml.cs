using SharedClassLibrary;
using System;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace PlayerSide;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        DataInnit();

        Globals.RService.ResourceChanged += OnMainCharaUpdateEvent;
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

