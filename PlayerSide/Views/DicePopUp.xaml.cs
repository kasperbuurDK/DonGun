using CommunityToolkit.Maui.Views;
using SharedClassLibrary;

namespace PlayerSide.Views;

public partial class DicePopUp : Popup
{
    private Dice _dice;
    public DicePopUp(string prefix, int from, int to)
    {
        InitializeComponent();
        InitialiseDice(prefix, from, to);
    }

    void OnDiceRolled(object sender, EventArgs e)
    {
        Close(_dice.Result);
    }

    void OnDiceRolling(object sender, EventArgs e)
    {
        ((MauiDice)sender).SetImage();
    }

    private void InitialiseDice(string prefix, int from, int to)
    {
        Image image = new()
        {
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        MauiDice dice = new(prefix, from, to, image);
        dice.RollingChanged += OnDiceRolling;
        dice.Rolled += OnDiceRolled;
        _dice = dice;
        Button button = new() { Opacity = 0, Padding = 1 };
        mainGrid.Add(image);
        mainGrid.Add(button);
    }

    void OnCancelButtonClicked(object sender, EventArgs e) => Close(null);
}