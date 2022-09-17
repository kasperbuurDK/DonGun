using SharedClassLibrary;

namespace PlayerSide;

public partial class DemoPage : ContentPage
{
	public DemoPage()
	{
        InitializeComponent();

    }

    private float _count;

    public float Count
    {
        set
        {
            if (value < 0)
                _count = 0;
            else
                _count = value;
        }
        get { return _count; }
    }

    private void OnCounterPosClicked(object sender, EventArgs e)
    {
        Count += 0.1F;
        CharPc.UpdateBars(Count, Count);
    }

    private void OnCounterNegClicked(object sender, EventArgs e)
    {
        Count -= 0.1F;
        CharPc.UpdateBars(Count, Count);
    }
}