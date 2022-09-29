
using SharedClassLibrary;
using System;

namespace PlayerSide;

public partial class BlobStatView : ContentView
{
    public static readonly BindableProperty borderImage = BindableProperty.Create(nameof(Border), typeof(string), typeof(BlobStatView), "", BindingMode.TwoWay);
    public static readonly BindableProperty CharHpProperty = BindableProperty.Create(nameof(CharHp), typeof(float), typeof(BlobStatView), 0F);
    public BlobStatView()
	{
		InitializeComponent();
	}

    public float CharHp
    {
        get => (float)GetValue(CharHpProperty);
        set
        {
            SetValue(CharHpProperty, value);
        }
    }
    public string Border
    {
        get => (string)GetValue(borderImage);
        set => SetValue(borderImage, value);
    }
}