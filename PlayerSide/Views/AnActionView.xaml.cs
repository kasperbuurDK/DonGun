using SharedClassLibrary;
using SharedClassLibrary.Actions;
using System.ComponentModel;

namespace PlayerSide.Views;

public partial class AnActionView : ContentView
{
    public AnActionView(IAnAction action)
    {
        InitializeComponent();
        Res.Text = action.SenderSignature;
        Send.Text = action.RecieverSignature;
        Chance.Text = action.ChanceToSucced.ToString();
        Sign.Text = action.Signature;
    }

    /**
        public string SenderSignature { get; set; }
        public string RecieverSignature { get; set; }
        public int ChanceToSucced { get; set; }
        public string Signature { get; init; }
     **/
}