using Microsoft.Maui.Controls;
using SharedClassLibrary;

namespace PlayerSide.Pages;

public partial class ImagePage : ContentPage
{
    private Border priSelected = new();
    private MauiPlayer player;
    public ImagePage(MauiPlayer p)
	{
		InitializeComponent();
        player = p;
        LoadImagesAsync();

    }

    public void LoadImagesAsync()
    {
        string[] imageNames = { "female_blonde.jpg", "female_blonde_young.jpg", "female_dark.jpg", "female_white.jpg", "male_blonde.jpg", "male_dark.jpg" };
        foreach (string img in imageNames)
        {
            ImageButton Ibutton = new() {Padding = 3, Source = img, Scale=1 , HeightRequest = 200 };
            Border border = new()
            {
                Stroke = Color.Parse("Transparent"),
                Background = Color.Parse("Transparent"),
                StrokeThickness = 5,
                Margin = new Thickness(70, 1.5, 70, 1.5),
                Content = Ibutton
            };
            Ibutton.Clicked += delegate (object sender, EventArgs e)
            {
                if (!border.Equals(priSelected))
                {
                    border.Stroke = Application.Current.Resources.MergedDictionaries.First()["Primary"] as Color;
                    player.ImageName = img;
                    priSelected.Stroke = Color.Parse("Transparent");
                    priSelected = border;
                }
            };
            ImageStackLayout.Add(border);
        }
    }
}