using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private RestService<Dictionary<int, Player>, Player> _restService { get; set; }
    private List<int> _keys { get; set; }
    private IConfiguration _configuration;

    public SheetPage()
    {
        InitializeComponent();
        _restService = new(MauiProgram.RestUserInfo.BaseUrl, MauiProgram.RestUserInfo.AuthHeader);
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
        UpdateSheetsAsync();
    }

    public async void UpdateSheetsAsync()
    {
        Settings settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
        await _restService.RefreshDataAsync(settings.RestUriSheet + MauiProgram.RestUserInfo.UserName);
        _keys = new();
        foreach(KeyValuePair<int, Player> p in _restService.ReturnStruct) 
        {
            // Needs Bindings
            SheetStackLayout.Add(new CharView() { CharName = p.Value.Name, Character = p.Value});
            _keys.Add(p.Key);
        }
    }
}

