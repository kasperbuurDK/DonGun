namespace PlayerSide.Views;

public partial class SheetView : ContentView
{
	public SheetView(MauiPlayer player)
	{
		InitializeComponent();
        if (string.IsNullOrEmpty(player.ImageName))
            player.ImageName = "no_data.png";
        CharaImg.Source = player.ImageName;
        NameText.Text = player.Name;
        IconDex.Text = player.Dexterity.ToString();
        IconStr.Text = player.Strength.ToString();
        IconWis.Text = player.Wisdome.ToString();
        IconInt.Text = player.Intelligence.ToString();
        IconCon.Text = player.Constitution.ToString();
        IconCha.Text = player.Charisma.ToString();
    }
}