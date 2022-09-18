namespace PlayerSide;

public partial class DiceSelectView : ContentView
{
	public readonly Grid MainGrid;
	public DiceSelectView()
	{
		InitializeComponent();
		MainGrid = mainGrid;
	}
}