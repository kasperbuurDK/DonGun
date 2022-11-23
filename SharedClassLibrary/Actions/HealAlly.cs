namespace SharedClassLibrary.Actions
{
    public class HealAlly : HelperAction
    {
        public HealAlly(string senderSig, string recieverSig) : base(senderSig, recieverSig)
        {
        }

        public override bool MakeBasicAction(int diceValue, Character sender, Character reciever)
        {
            reciever.RecieveHealing(sender.CalculateHealing(diceValue));
            return true;
        }
    }
}
