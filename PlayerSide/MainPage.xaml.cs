namespace PlayerSide;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Globals.RService.ResponseResived += RestClient_ResponseResived;
    }

    private void RestClient_ResponseResived(object sender, EventArgs e)
    {
        //respLabel.Text = Globals.RService.Response.ToString() + Globals.RService.Items.ToString();
    }

    private void OnCounterPosClicked(object sender, EventArgs e)
    {
        _ = Globals.RService.RefreshDataAsync("/api/weatherforecast/Get/user");
    }

    private void OnCounterNegClicked(object sender, EventArgs e)
    {
        string outPrint = Globals.RService.Response?.ToString() + Globals.RService.Items?.ToArray().ToString();
        //respLabel.Text = outPrint;
    }
}

