using SharedClassLibrary;
using System;

namespace PlayerSide;

public partial class DicePage : ContentPage
{
    private D20 _d20;
    public DicePage()
    {
        InitializeComponent();
        InitializeDice();
    }

    private void OnRollClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }

    private void InitializeDice()
    {
        _d20 = new D20();
        _d20.RollingChanged += OnDiceRollingChanged;
        _d20.Rolled += OnDiceRolled;
    }

    void OnDiceRolled(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = true;
    }

    void OnDiceRollingChanged(object sender, EventArgs e)
    {
        DiceRolls.Source = _d20.ImageName();
    }

    private void OnAdd8BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnAdd4BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnAdd10BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnAdd00BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnAdd6BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnAdd12BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnAdd20BtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnRemoveDiceBtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
    private void OnClearDiceBtnClicked(object sender, EventArgs e)
    {
        RollBtn.IsEnabled = false;
        DiceRolls.RelRotateTo(360, (uint)_d20.RollingDuration.TotalMilliseconds, Easing.CubicOut);
        _d20.Roll();
    }
}