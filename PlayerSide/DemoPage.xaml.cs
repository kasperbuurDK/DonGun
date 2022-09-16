using SharedClassLibrary;

namespace PlayerSide;

public partial class DemoPage : ContentPage
{
	public DemoPage()
	{
        InitializeComponent();
        InitializeDice();
    }

    private D20 _d20;
    private float _count;

    public float Count
    {
        set
        {
            if (value < 0)
                _count = 0;
            else
                _count = value;
        }
        get { return _count; }
    }

    private void OnCounterPosClicked(object sender, EventArgs e)
    {
        Count += 0.1F;
        CharPc.UpdateBars(Count, Count);
    }

    private void OnCounterNegClicked(object sender, EventArgs e)
    {
        Count -= 0.1F;
        CharPc.UpdateBars(Count, Count);
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
}