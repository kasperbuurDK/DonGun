using SharedClassLibrary.Actions;

namespace SharedClassLibrary.MessageStrings
{
    public class ActionMessage : Message
    {
        // Maui can't return the action, BC of the serializing and deserializing.
        // Retruns the string signature insted.
        // IAnAction -> string
        public string ActionToPerform { get; set; } = string.Empty;

        public int DiceValue { get; set; } = default;

        public ActionMessage(string SKey, string anAction, int diceResult) : base(MessageType.ActionEvent, SKey)
        {
            ActionToPerform = anAction;
            DiceValue = diceResult;
        }
        public ActionMessage() : base(MessageType.ActionEvent)
        {
        }


        public override string ToString()
        {
            return string.Format($"{base.ToString()} - ");
        }
    }
}
