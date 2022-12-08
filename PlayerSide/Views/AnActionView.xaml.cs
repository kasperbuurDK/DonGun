using SharedClassLibrary;
using SharedClassLibrary.Actions;
using System.ComponentModel;

namespace PlayerSide.Views;

public partial class AnActionView : ContentView
{
    public AnActionView(AnAction action)
    {
        InitializeComponent();
        Res.Text = action.SenderSignature;
        Send.Text = action.RecieverSignature;
        if (MauiProgram.GameOrder is not null)
        {
            Character target = MauiProgram.GameOrder.Where(c => c.Signature == action.RecieverSignature).First();
            Res.Text = target.Name;
            if (string.IsNullOrEmpty(target.ImageName))
                resImg.Source = "no_data.png";
            else
                resImg.Source = target.ImageName;
            Character sender = MauiProgram.GameOrder.Where(c => c.Signature == action.SenderSignature).First();
            Res.Text = sender.Name;
            if (string.IsNullOrEmpty(sender.ImageName))
                resImg.Source = "no_data.png";
            else
                resImg.Source = sender.ImageName;

            if (target.Team == sender.Team)
                UpdateFrame.BackgroundColor = Color.FromRgba(26, 178, 54, 80); // Workaround - Green
            else
                UpdateFrame.BackgroundColor = Color.FromRgba(190, 25, 54, 80); // Workaround - Red
        } else
            UpdateFrame.BackgroundColor = Color.FromRgba(255, 255, 255, 80); // Workaround - White
    }
}