using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private RestService<Dictionary<int, MauiPlayer>, MauiPlayer> _restService { get; set; }
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
        try
        {
            Globals.Connectivity = _restService.ReturnStruct.First().Value;
            foreach (KeyValuePair<int, MauiPlayer> p in _restService.ReturnStruct)
            {
                //SheetStackLayout.Add(new CharView(p.Value));
                _keys.Add(p.Key);
            }
        } catch (Exception ex)
        {
            // No elements
        }
    }
}

