using SharedClassLibrary.Actions;

namespace SharedClassLibrary.MessageStrings
{
    public class ActionMessage : Message
    {
        // Maui can't return the action, BC of the serializing and deserializing.
        // Retruns the string signature insted.
        // IAnAction -> string
        public string ActionToPerform;

        public int DiceValue;

        public ActionMessage(string SKey, string anAction, int diceResult) : base(MessageType.ActionEvent, SKey)
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
