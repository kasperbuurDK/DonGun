namespace SharedClassLibrary.Actions
{
    public interface IAnAction
    {
        public string SenderSignature { get; set; }
        public string RecieverSignature { get; set; }
        public int ChanceToSucced { get; set; }
        public string Signature { get; init; }
        public bool MakeBasicAction(int diceValue, Character sender, Character reciever);

    }

}