namespace PlayerSide;

public partial class MainPage : ContentPage
{
	float count = 0F;

    public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterPosClicked(object sender, EventArgs e)
	{
		count += 0.1F;
        OpdateCountLabel();
        await progressBar1.ProgressTo(count, 500, Easing.Linear);
    }

    private async void OnCounterNegClicked(object sender, EventArgs e)
    {
        count -= 0.1F;
        OpdateCountLabel();
        await progressBar1.ProgressTo(count, 250, Easing.CubicOut);
    }

    private void OpdateCountLabel()
    {
        ProgressCount.Text = $"{count}";

        SemanticScreenReader.Announce(ProgressCount.Text);
    }

}

