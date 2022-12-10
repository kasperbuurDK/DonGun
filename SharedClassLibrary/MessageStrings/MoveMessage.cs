namespace SharedClassLibrary.MessageStrings
{
    public class MoveMessage : Message
    {
        public string Direction { get; set; } = string.Empty;
        public int Distence { get; set; } = default;

        public MoveMessage() : base(MessageType.MoveEvent) { }

        public MoveMessage(string SKey) : base(MessageType.MoveEvent, SKey) { }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {Direction} - {Distence}");
        }
    }
}
