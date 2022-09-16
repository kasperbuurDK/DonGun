namespace PlayerSide;
using SharedClassLibrary;

public partial class MainPage : ContentPage
{

    /* TODO:
     *      Own Page for dice rolls.
     *      Add more dices types
     *      Add dice Cup. (up to 6 dices (diff types))
     *          Needs Cup to JSON for REST
     *      Add number of dices selection option.
     *      
     */

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
    public MainPage()
	{
		InitializeComponent();
        InitializeDice();
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

