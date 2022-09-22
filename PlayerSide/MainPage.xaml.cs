using System;
using System.Linq;

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
        RestClient = new RestService<string>();
    }

    private RestService<string> _restClient;

    public RestService<string> RestClient
    {
        set
        {
            _restClient = value;
            _restClient.ResponseResived += RestClient_ResponseResived;
        }
        get { return _restClient; }
    }

    private void RestClient_ResponseResived(object sender, EventArgs e)
    {
        respLabel.Text = RestClient.Response.ToString() + RestClient.Items.ToArray().ToString(); ;
    }

    private void OnCounterPosClicked(object sender, EventArgs e)
    {
        _ = RestClient.RefreshDataAsync("/api/values");
    }

    private void OnCounterNegClicked(object sender, EventArgs e)
    {
        string outPrint = RestClient.Response?.ToString() + RestClient.Items?.ToArray().ToString();
        respLabel.Text = outPrint;
    }
}

