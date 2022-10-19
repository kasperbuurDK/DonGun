using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private RestService<Dictionary<int, Player>, Player> _restService { get; set; }
    private List<int> _keys { get; set; }

    public SheetPage()
    {
        InitializeComponent();
        _restService = new(Globals.RestUserInfo.AuthHeader);
        UpdateSheetsAsync();
        //Globals.FileUpdateHub.PropertyChangedEvent += OnUpdateEvent;
    }

    public async void UpdateSheetsAsync()
    {
        await _restService.RefreshDataAsync(Constants.RestUriSheet + Globals.RestUserInfo.UserName);
        _keys = new();
        foreach(KeyValuePair<int, Player> p in _restService.ReturnStruct) 
        {
            SheetStackLayout.Add(new CharView() { CharName = p.Value.Name, Character = p.Value});
            _keys.Add(p.Key);
        }
    }

    private void DataInnit()
    {
        Random rnd = new();
        // NameText.Text = (Player)Globals.Connectivity.Name;
        // CharaImg.Source = Globals.Connectivity.Img;
        /*IconDex.Text = Globals.Connectivity.Dexterity.ToString();
        IconStr.Text = Globals.Connectivity.Strength.ToString();
        IconWis.Text = Globals.Connectivity.Wisdome.ToString() + rnd.Next(1,50).ToString();
        IconInt.Text = Globals.Connectivity.Intelligence.ToString();
        IconCon.Text = Globals.Connectivity.Constitution.ToString();
        IconCha.Text = Globals.Connectivity.Charisma.ToString();*/
    }

    private async void LoadSheetBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserPage());
    }
}

