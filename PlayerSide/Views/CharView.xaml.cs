using SharedClassLibrary;

namespace PlayerSide.Views;

public partial class CharView : ContentView
{
    public static readonly BindableProperty CharHpProperty = BindableProperty.Create(nameof(CharHp), typeof(float), typeof(CharView), 0F);
    public static readonly BindableProperty CharResProperty = BindableProperty.Create(nameof(CharRes), typeof(float), typeof(CharView), 0F);
    public static readonly BindableProperty CharNameProperty = BindableProperty.Create(nameof(CharName), typeof(string), typeof(CharView), "");


    private Character_abstract _character;
    public Character_abstract Character
    {
        get => _character;
        set
        {
            _character = value;
            CharHp = _character.HealthCurrent;
            CharRes = _character.ResourceCurrent;
            // TODO: Add Character profile image
        }
    }

    public float CharHp
    {
        get => (float)GetValue(CharHpProperty);
        set
        {
            HpBarText.Text = string.Format($"{value} / {_character.Health}");
            SetValue(CharHpProperty, value);
            float preHpVal = (float)progressBarHp.Progress;
            float hpVal = value.Remap(0, _character.Health, 0F, 1.0F);
            if (hpVal > preHpVal)
                progressBarHp.ProgressTo(hpVal, 250, Easing.Linear);
            else
                progressBarHp.ProgressTo(hpVal, 250, Easing.CubicInOut);
        }
    }

    public float CharRes
    {
        get => (float)GetValue(CharResProperty);
        set
        {
            ResBarText.Text = string.Format($"{value} / {_character.Resource}");
            SetValue(CharResProperty, value);
            float preResVal = (float)progressBarRes.Progress;
            float resVal = value.Remap(0, _character.Resource, 0F, 1.0F);
            if (resVal > preResVal)
                progressBarRes.ProgressTo(resVal, 250, Easing.Linear);
            else
                progressBarRes.ProgressTo(resVal, 250, Easing.CubicInOut);
        }
    }

    public string CharName
    {
        get => (string)GetValue(CharNameProperty);
        set => SetValue(CharNameProperty, value);
    }

    public CharView()
    {
        InitializeComponent();
    }
}