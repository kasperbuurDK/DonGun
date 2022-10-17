using PlayerSide.Views;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class UserPage : ContentPage
{
    private RestService<Dictionary<int, Player>, Player> _restService { get; set; }
    private int? _selected { get; set; } = null;

    public UserPage()
	{
		InitializeComponent();
        _restService = new(Globals.RestUserInfo.AuthHeader);
        UpdateSheetsAsync();
    }

    private void AddSheetBtnClicked(object sender, EventArgs e)
    {
        AddSheet.Text = $"{_selected}";
    }

    private void RefreshBtnClicked(object sender, EventArgs e)
    {
        UpdateSheetsAsync();
    }

    public async void UpdateSheetsAsync()
    {
        PageLock(true);
        await _restService.RefreshDataAsync(Constants.RestUriSheet + Globals.RestUserInfo.UserName);
        SheetStackLayout.Clear();
        _selected = null;
        foreach (KeyValuePair<int, Player> p in _restService.ReturnStruct)
        {
            Grid grid = new();
            grid.Add(new CharView { CharName = p.Value.Name, Character = p.Value });
            Button button = new() { Opacity = 0 };
            button.Clicked += (sender, args) => _selected = p.Key;
            grid.Add(button);
            SheetStackLayout.Add(grid);
        }
        PageLock(false);
    }

    private void PageLock(bool l)
    {
        ActivityIndicator.IsRunning = l;
        RefreshSheet.IsEnabled = !l;
    }
}