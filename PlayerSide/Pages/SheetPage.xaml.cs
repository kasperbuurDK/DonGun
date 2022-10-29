using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private RestService<Dictionary<int, MauiPlayer>, MauiPlayer> _restService { get; set; }
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
        try
        {
            MauiProgram.Connectivity = _restService.ReturnStruct.First().Value;
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

