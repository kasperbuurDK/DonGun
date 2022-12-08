using SharedClassLibrary.Actions;

namespace SharedClassLibrary.MessageStrings
{
    public class UpdateMessage : Message
    {
        //***LIST ARE NOT VALID IN SIGNALR***
        // Serialize the list before hand
        // List<IAnAction> -> string
        public string PossibleActionsJson { get; set; } = string.Empty;
        public string UpdateStr { get; set; } = string.Empty;

        public UpdateMessage() : base(MessageType.UpdateEvent) { }

        public UpdateMessage(string SKey) : base(MessageType.UpdateEvent, SKey) { }

        public UpdateMessage(string SKey, List<IAnAction> list) : this(SKey) 
        {
            PossibleActionsJson = list.TypeToJson();
        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {UpdateStr}");
        }
    }
}
