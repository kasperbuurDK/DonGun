using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using PlayerSide.Views;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class UserPage : ContentPage
{
    private int? _selected;
    private Border priSelected = new();
    private Dictionary<int, MauiPlayer> _sheetDict;
    private IConfiguration _configuration;

    public UserPage()
	{
		InitializeComponent();
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
        UpdateSheetsAsync();
    }

    private async void AddSheetBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ModSheetPage(UpdateSheetsAsync));
    }

    private async void ModSheetBtnClicked(object sender, EventArgs e)
    {
        if (_selected != null && _sheetDict != null)
        {
            await Navigation.PushAsync(new ModSheetPage(_sheetDict[(int)_selected], (int)_selected, UpdateSheetsAsync));
        }
    }

    private void RefreshBtnClicked(object sender, EventArgs e)
    {
        UpdateSheetsAsync();
    }

    public async void UpdateSheetsAsync()
    {
        PageLock(true);
        Settings settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
        string authHeader = await SecureStorage.Default.GetAsync("authHeader");
        string user = await SecureStorage.Default.GetAsync("username");

        if (authHeader is not null && user is not null)
        {
            RestService<Dictionary<int, MauiPlayer>, MauiPlayer> restService = new(new Uri(settings.BaseUrl), authHeader);
            await restService.RefreshDataAsync(settings.RestUriSheet + user);
            SheetStackLayout.Clear();
            _selected = null;
            foreach (KeyValuePair<int, MauiPlayer> p in restService.ReturnStruct)
            {
                Grid grid = new()
                {
                new CharView(p.Value)
                };
                Button button = new() { Opacity = 0, Padding = 1 };
                Border border = new()
                {
                    Stroke = Color.Parse("Transparent"),
                    Background = Color.Parse("Transparent"),
                    StrokeThickness = 5,
                    Margin = new Thickness(1.5, 1.5, 1.5, 1.5),
                    Content = button
                };
                button.Clicked += delegate (object sender, EventArgs e)
                {
                    if (!border.Equals(priSelected))
                    {
                        border.Stroke = Application.Current.Resources.MergedDictionaries.First()["Primary"] as Color;
                        _selected = p.Key;
                        priSelected.Stroke = Color.Parse("Transparent");
                        priSelected = border;
                    }
                };
                grid.Add(border);
                SheetStackLayout.Add(grid);
            }
            _sheetDict = restService.ReturnStruct;
        }
        PageLock(false);
    }


    private void PageLock(bool l)
    {
        ActivityIndicator.IsRunning = l;
        RefreshSheet.IsEnabled = !l;
        AddSheet.IsEnabled = !l;
        ModSheet.IsEnabled = !l;
    }
}