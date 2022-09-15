namespace PlayerSide;
using SharedClassLibrary;

public partial class MainPage : ContentPage
{
    private D20 _d20;
    private float count = 0F;

    public MainPage()
	{
		InitializeComponent();
        InitializeDice();

    }

	private void OnCounterPosClicked(object sender, EventArgs e)
	{
		count += 0.1F;
        CharPc.UpdateBars(count,count);
        OpdateCountLabel();

    }

    private void OnCounterNegClicked(object sender, EventArgs e)
    {
        count -= 0.1F;
        CounterBtnNeg.IsEnabled = false;
        _d20.Roll();
        CharPc.UpdateBars(count, count);
        OpdateCountLabel();
    }

    private void OpdateCountLabel()
    {
        ProgressCount.Text = $"{count}";
        SemanticScreenReader.Announce(ProgressCount.Text);
    }
    private void InitializeDice()
    {
        _d20 = new D20();
        _d20.RollingChanged += OnDiceRollingChanged;
        _d20.Rolled += OnDiceRolled;

    }

    void OnDiceRolled(object sender, EventArgs e)
    {
        CounterBtnNeg.IsEnabled = true;
    }

    void OnDiceRollingChanged(object sender, EventArgs e)
    {
        // ToDo: Select desired picture from image list depending on _Dice.Result
        DiceRollLabel.Text = _d20.Result.ToString();
        SemanticScreenReader.Announce(DiceRollLabel.Text);
    }
}

