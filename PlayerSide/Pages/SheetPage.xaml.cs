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
    }

    public async void UpdateSheetsAsync()
    {
        await _restService.RefreshDataAsync(Constants.RestUriSheet + Globals.RestUserInfo.UserName);
        _keys = new();
        foreach(KeyValuePair<int, Player> p in _restService.ReturnStruct) 
        {
            // Needs Bindings
            SheetStackLayout.Add(new CharView() { CharName = p.Value.Name, Character = p.Value});
            _keys.Add(p.Key);
        }
    }
}

