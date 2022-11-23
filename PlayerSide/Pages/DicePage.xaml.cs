using SharedClassLibrary;
using SharedClassLibrary.MessageStrings;

namespace PlayerSide.Pages;

public partial class DicePage : ContentPage
{
    private readonly Cup _cup;

    public DicePage()
    {
        InitializeComponent();
        _cup = new Cup(8);
        _cup.Rolled += OnCupRolled;
    }
    async void OnCupRolled(object sender, EventArgs e)
    {
        if (MauiProgram.Hub is not null && MauiProgram.Hub.IsConnected)
            await MauiProgram.Hub.Send(new ActionMessage(_cup));
        ToggleIsEnable(true);
    }

    void OnDiceRolled(object sender, EventArgs e)
    {
        // UI/Main Thread must handle all roles redraws...
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
        dice.RollingChanged += OnDiceRolled;
        _cup.Add(dice);
        int row = (int)Math.Floor((float)(_cup.DiceList.Count - 1) / 2F);
        int col = (_cup.DiceList.Count % 2);
        DiceView.MainGrid.Add(image, col, row);
    }

    private void OnAdd8BtnClicked(object sender, EventArgs e)
    {
        InitialiseDice("deight", 1, 8);
    }

    private void OnAdd4BtnClicked(object sender, EventArgs e)
    {
        InitialiseDice("dfour", 1, 4);
    }
    private void OnAdd10BtnClicked(object sender, EventArgs e)
    {
        InitialiseDice("dhundred", 1, 10);
    }

    private void OnAdd6BtnClicked(object sender, EventArgs e)
    {
        InitialiseDice("dsix", 1, 6);
    }
    private void OnAdd12BtnClicked(object sender, EventArgs e)
    {
        InitialiseDice("dtwelve", 1, 12);
    }
    private void OnAdd20BtnClicked(object sender, EventArgs e)
    {
        InitialiseDice("dtwenty", 1, 20);
    }
    private void OnRemoveDiceBtnClicked(object sender, EventArgs e)
    {
        RemoveDiceFromPage();
    }
    private void OnClearDiceBtnClicked(object sender, EventArgs e)
    {
        while (_cup.DiceList.Count > 0)
        {
            RemoveDiceFromPage();
        }
    }
    private void OnRollClicked(object sender, EventArgs e)
    {
        if (_cup.DiceList.Count > 0)
        {
            ToggleIsEnable(false);
            _cup.RollCup();
        }
    }

    private void RemoveDiceFromPage()
    {
        MauiDice temp = (MauiDice)_cup.Remove();
        if (temp != null)
            DiceView.MainGrid.Remove(temp.DiceImage);
    }

    private void ToggleIsEnable(bool e)
    {
        // No dice softlocks!
        RollBtn.IsEnabled = e;
        Add00Btn.IsEnabled = e;
        Add10Btn.IsEnabled = e;
        Add12Btn.IsEnabled = e;
        Add20Btn.IsEnabled = e;
        Add4Btn.IsEnabled = e;
        Add6Btn.IsEnabled = e;
        Add8Btn.IsEnabled = e;
        ClearDiceBtn.IsEnabled = e;
        RemoveDiceBtn.IsEnabled = e;
    }
}
