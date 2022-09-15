using System;

namespace PlayerSide;

public partial class CharView : ContentView
{
    public static readonly BindableProperty CharHpProperty = BindableProperty.Create(nameof(CharHp), typeof(float), typeof(CharView), 0F);
    public static readonly BindableProperty CharResProperty = BindableProperty.Create(nameof(CharRes), typeof(float), typeof(CharView), 0F);

    public float CharHp
    {
        get => (float)GetValue(CharHpProperty);
        set => SetValue(CharHpProperty, value);
    }

    public float CharRes
    {
        get => (float)GetValue(CharResProperty);
        set => SetValue(CharResProperty, value);
    }

    public async void UpdateBars(float hpVal, float resVal) 
    {
        float preHpVal = (float)progressBarHp.Progress;
        float preResVal = (float)progressBarRes.Progress;
        if (hpVal > preHpVal)
            await progressBarHp.ProgressTo(hpVal, 250, Easing.Linear);
        else
            await progressBarHp.ProgressTo(hpVal, 250, Easing.CubicInOut);
        if (resVal > preResVal)
            await progressBarRes.ProgressTo(resVal, 250, Easing.Linear);
        else
            await progressBarRes.ProgressTo(resVal, 250, Easing.CubicInOut);
    }

    public CharView()
	{
		InitializeComponent();
	}
}