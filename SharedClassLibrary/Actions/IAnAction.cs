namespace SharedClassLibrary.Actions
{
    public interface IAnAction
    {
        public Character Sender { get; set; }
        public Character Reciever { get; set; }
        public int ChanceToSucced { get; set; }

        public bool MakeBasicAction(int diceValue);
    }

}