namespace SharedClassLibrary.MessageStrings
{
    public class NewTurnMessage : Message
    {
        //***LIST ARE NOT VALID IN SIGNALR***
        // Serialize the list before hand
        // Queue<Character> -> string
        public string TheQueue { get; set; } = string.Empty;
        //***LIST ARE NOT VALID IN SIGNALR***
        // Serialize the list before hand
        // List<string> -> string
        public string Happenings { get; set; } = string.Empty;

        public NewTurnMessage() : base(MessageType.NewTurn)
        {
        }

        public NewTurnMessage(string sessionKey, Queue<Character> queue, List<string> happenings) : base(MessageType.NewTurn, sessionKey)
        {
            var asList = new List<Character>(queue);
            TheQueue = asList.TypeToJson().Compress();
            Happenings = happenings.TypeToJson().Compress();
        }

        public void Decompress()
        {
            TheQueue = TheQueue.Decompress();
            Happenings = Happenings.Decompress();
        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - New Turn Started {DateTime.Now}");
        }

    }
}


