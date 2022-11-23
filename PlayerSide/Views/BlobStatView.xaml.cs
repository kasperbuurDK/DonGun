namespace PlayerSide.Views;

public partial class BlobStatView : ContentView
{
    public static readonly BindableProperty borderImage = BindableProperty.Create(nameof(Border), typeof(string), typeof(BlobStatView), defaultValue: string.Empty, defaultBindingMode: BindingMode.OneWay, propertyChanged: ImageBorderChanged);
    public static readonly BindableProperty textImage = BindableProperty.Create(nameof(Text), typeof(string), typeof(BlobStatView), defaultValue: string.Empty, defaultBindingMode: BindingMode.OneWay, propertyChanged: ImageTextChanged);


    private static void ImageBorderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        BlobStatView control = (BlobStatView)bindable;
        control.ImageBorder.Source = newValue?.ToString();
    }
    private static void ImageTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        BlobStatView control = (BlobStatView)bindable;
        control.ValueText.Text = newValue?.ToString();
    }
    public string Border
    {
        get => (string)GetValue(borderImage);
        set => SetValue(borderImage, value);
    }
    public string Text
    {
        get => (string)GetValue(textImage);
        set => SetValue(textImage, value);
    }
    public BlobStatView()
    {
        InitializeComponent();
    }
}