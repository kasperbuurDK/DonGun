namespace SharedClassLibrary.MessageStrings
{
    public class NewTurnMessage : Message
    {
        //***LIST ARE NOT VALID IN SIGNALR***
        // Serialize the list before hand
        // Queue<Character> -> string
        public string TheQueue { get; set; }
        //***LIST ARE NOT VALID IN SIGNALR***
        // Serialize the list before hand
        // List<string> -> string
        public string Happenings { get; set; }7


        public NewTurnMessage(string sessionKey, Queue<Character> queue, List<string> happenings) : base(MessageType.NewTurn, sessionKey)
        {
            TheQueue = queue.TypeToJson();
            Happenings = happenings.TypeToJson();
        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - New Turn Started {DateTime.Now}");
        }

    }
}


