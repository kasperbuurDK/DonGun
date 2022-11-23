using SharedClassLibrary.Actions;

namespace SharedClassLibrary.MessageStrings
{
    public class ActionMessage : Message
    {
        public IAnAction ActionToPerform;

        public int DiceValue;

        public ActionMessage(string SKey, IAnAction anAction, int diceResult) : base(MessageType.ActionEvent, SKey)
        {
            ActionToPerform = anAction;
            DiceValue = diceResult;
        }


        public override string ToString()
        {
            return string.Format($"{base.ToString()} - ");
        }
    }
}
