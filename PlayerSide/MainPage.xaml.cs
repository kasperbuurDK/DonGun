namespace PlayerSide;

public partial class MainPage : ContentPage
{
	int count = 0;

    public MainPage()
	{
		InitializeComponent();
	}

	private async void OnCounterPosClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
        await progressBar1.ProgressTo(count/10, 500, Easing.Linear);
    }

    private async void OnCounterNegClicked(object sender, EventArgs e)
    {
        count--;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
        await progressBar1.ProgressTo(count/10, 500, Easing.Linear);
    }

}

