using SharedClassLibrary;
using System.Collections.ObjectModel;

namespace PlayerSide.Pages;

public partial class ModSheetPage : ContentPage
{
    public MauiPlayer MPlayer { get; set; }

    public ModSheetPage()
    {
        InitializeComponent();
        MPlayer = new MauiPlayer();
        BindingContext = MPlayer;
    }

    private void BtnChangeColor_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void BtnAddCar_OnClick(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}