using Microsoft.Extensions.Configuration;
using PlayerSide.Views;
using SharedClassLibrary;
using System.Collections.Generic;

namespace PlayerSide.Pages;

public partial class SheetPage : ContentPage
{
    private List<int> _keys { get; set; }
    private IConfiguration _configuration;

    public SheetPage()
    {
        InitializeComponent();
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
        UpdateSheetsAsync();
    }

    public async void UpdateSheetsAsync()
    {
        Settings settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
        string authHeader = await SecureStorage.Default.GetAsync("authHeader");
        string user = await SecureStorage.Default.GetAsync("username");

        if (authHeader is not null && user is not null)
        {
            RestService<Dictionary<int, MauiPlayer>, MauiPlayer> restService = new(new Uri(settings.BaseUrl), authHeader);
            //await restService.RefreshDataAsync(settings.RestUriSheet + user);
            _keys = new();
            try
            {
                //MauiProgram.Connectivity = restService.ReturnStruct.First().Value;
                foreach (KeyValuePair<int, MauiPlayer> p in restService.ReturnStruct)
                {
                    //SheetStackLayout.Add(new CharView(p.Value));
                    _keys.Add(p.Key);
                }
            }
            catch (Exception ex)
            {
                // No elements
            }
        }
    }
}

