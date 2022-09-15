namespace PlayerSide;

public partial class MainPage : ContentPage
{
	float count = 0F;

    public MainPage()
	{
		InitializeComponent();
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
        CharPc.UpdateBars(count, count);
        OpdateCountLabel();
    }

    private void OpdateCountLabel()
    {
        ProgressCount.Text = $"{count}";
        SemanticScreenReader.Announce(ProgressCount.Text);
    }

}

