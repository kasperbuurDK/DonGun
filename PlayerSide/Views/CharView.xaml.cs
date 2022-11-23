using SharedClassLibrary;
using System.ComponentModel;

namespace PlayerSide.Views;

public partial class CharView : ContentView
{
    public void UpdateBars(object sender, PropertyChangedEventArgs e)
    {
        if (sender is MauiPlayer)
        {
            MauiPlayer player = (MauiPlayer)sender;
            if (e.PropertyName.Contains("Health"))
            {
                MainThread.BeginInvokeOnMainThread(() => UpdateHpBar(player));
            }
            else if (e.PropertyName.Contains("Resource"))
            {
                MainThread.BeginInvokeOnMainThread(() => UpdateResBar(player));
            }
        }
    }

    public void UpdateHpBar(MauiPlayer p)
    {
        HpBarText.Text = string.Format($"{p.HealthCurrent} / {p.HealthMax}");
        float preHpVal = (float)progressBarHp.Progress;
        float hpVal = p.HealthCurrent.Remap(0, p.HealthMax, 0F, 1.0F);
        if (hpVal > preHpVal)
            progressBarHp.ProgressTo(hpVal, 250, Easing.Linear);
        else
            progressBarHp.ProgressTo(hpVal, 250, Easing.CubicInOut);
    }

    public void UpdateResBar(MauiPlayer p)
    {
        ResBarText.Text = string.Format($"{p.ResourceCurrent} / {p.ResourceMax}");
        float preResVal = (float)progressBarRes.Progress;
        float resVal = p.ResourceCurrent.Remap(0, p.ResourceMax, 0F, 1.0F);
        if (resVal > preResVal)
            progressBarRes.ProgressTo(resVal, 250, Easing.Linear);
        else
            progressBarRes.ProgressTo(resVal, 250, Easing.CubicInOut);
    }

    public CharView(MauiPlayer _character)
    {
        InitializeComponent();
        UpdateHpBar(_character);
        UpdateResBar(_character);
        _character.PropertyChanged += UpdateBars;
        charaBinding.Character = _character;
        if (string.IsNullOrEmpty(charaBinding.Character.ImageName))
            charaBinding.Character.ImageName = "no_data.png";
    }
}