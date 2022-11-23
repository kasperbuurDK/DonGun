using CommunityToolkit.Maui.Views;

namespace PlayerSide.Views;

public partial class SelectPopUp : Popup
{
    private int? _selected;
    private Border priSelected = new();
    public SelectPopUp()
    {
        InitializeComponent();
        CreateContent();
    }

    private void CreateContent()
    {
        _selected = null;
        foreach (KeyValuePair<int, MauiPlayer> p in MauiProgram.Sheets)
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
    }

    void OnJoinButtonClicked(object sender, EventArgs e) => Close(_selected);

    void OnCancelButtonClicked(object sender, EventArgs e) => Close(null);
}