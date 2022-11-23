namespace SharedClassLibrary.Actions
{
    public class InspireAlly : HelperAction
    {
        public InspireAlly(string senderSig, string recieverSig) : base(senderSig, recieverSig)
        {
        }

        public override bool MakeBasicAction(int diceValue, Character sender, Character reciever)
        {
            reciever.RecieveInspiration(sender.CalculateInspiration(diceValue));
            return true;
        }
    }
}


