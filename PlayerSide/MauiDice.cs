using SharedClassLibrary;

namespace PlayerSide
{
    public class MauiDice : Dice
    {
        public readonly string Prefix;
        private readonly Random _rand = new Random();

        public Image DiceImage { get; set; }
        public MauiDice(string prefix, int from, int to, Image img, int timeSpan = 2000)
            : base(to, from)
        {
            RollingDuration = new TimeSpan(0, 0, 0, 0, timeSpan);
            Prefix = prefix;
            DiceImage = img;
            SetImage();
        }
        public override void Roll()
        {
            DiceImage.RelRotateTo(_rand.Next(0, 2) == 0 ? 360 * 4 : -360 * 4, ((uint)RollingDuration.TotalMilliseconds) - 50 + (uint)_rand.Next(0, 25), Easing.CubicOut);
            base.Roll();
        }

        public void SetImage()
        {
            DiceImage.Source = String.Format($"{Prefix}_d{(Globals.NumAsAlpha)Result}roll.png");
        }
    }
}