using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using PlayerSide.Views;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class UserPage : ContentPage
{
    private RestService<Dictionary<int, MauiPlayer>, MauiPlayer> _restService { get; set; }
    private int? _selected;
    private Border priSelected = new();
    private IConfiguration _configuration;

    public UserPage()
	{
		InitializeComponent();
        _restService = new(MauiProgram.RestUserInfo.BaseUrl, MauiProgram.RestUserInfo.AuthHeader);
        _configuration = MauiProgram.Services.GetService<IConfiguration>();
        UpdateSheetsAsync();
    }

    private async void AddSheetBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ModSheetPage(UpdateSheetsAsync));
    }

    private async void ModSheetBtnClicked(object sender, EventArgs e)
    {
        if (_selected != null)
        {
            await Navigation.PushAsync(new ModSheetPage(_restService.ReturnStruct[(int)_selected], (int)_selected, UpdateSheetsAsync));
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
        await _restService.RefreshDataAsync(settings.RestUriSheet + MauiProgram.RestUserInfo.UserName);
        SheetStackLayout.Clear();
        _selected = null;
        foreach (KeyValuePair<int, MauiPlayer> p in _restService.ReturnStruct)
        {
            Grid grid = new()
            {
                new CharView { CharName = p.Value.Name, Character = p.Value }
            };
            Button button = new() { Opacity = 0, Padding = 5 };
            Border border = new()
            {
                Stroke = Color.Parse("Transparent"),
                Background = Color.Parse("Transparent"),
                StrokeThickness = 5,
                Margin = new Thickness(10,10,10,10),
                Content = button
            };
            button.Clicked += delegate (object sender, EventArgs e)
            {
                if (!border.Equals(priSelected))
                {
                    border.Stroke = (Color)Application.Current.Resources.MergedDictionaries.First()["Primary"];
                    _selected = p.Key;
                    priSelected.Stroke = Color.Parse("Transparent");
                    priSelected = border;
                }
            };
            grid.Add(border);
            SheetStackLayout.Add(grid);
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