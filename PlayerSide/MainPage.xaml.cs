namespace PlayerSide;

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
    public MainPage()
    {
        InitializeComponent();
        Globals.RService.ResponseResived += RestClient_ResponseResived;
    }

    private void RestClient_ResponseResived(object sender, EventArgs e)
    {
        respLabel.Text = Globals.RService.Response.ToString() + Globals.RService.Items.ToArray().ToString();
    }

    private void OnCounterPosClicked(object sender, EventArgs e)
    {
        _ = Globals.RService.RefreshDataAsync("/api/weatherforecast/Get/user");
    }

    private void OnCounterNegClicked(object sender, EventArgs e)
    {
        string outPrint = Globals.RService.Response?.ToString() + Globals.RService.Items?.ToArray().ToString();
        respLabel.Text = outPrint;
    }
}

