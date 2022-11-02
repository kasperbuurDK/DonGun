using System.Windows.Input;

namespace PlayerSide.Pages;

public partial class MovePage : ContentPage
{
    public ICommand ArrowCommand { private set; get; }
    private string _selected;
    public MovePage()
    {
        InitializeComponent();
        ArrowCommand = new Command<string>(
        execute: (string arg) =>
        {
            if (arg != "done")
                _selected = arg;
            else
            {
                //Take move amount from picker (missing) and send over hub to Dun.
            }
        });
    }
}