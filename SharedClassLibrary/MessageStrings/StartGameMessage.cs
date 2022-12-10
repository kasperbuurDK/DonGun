namespace SharedClassLibrary.MessageStrings
{
    public class StartGameMessage : Message
    {
        public StartGameMessage() : base(MessageType.StartGame)
        {
        }

        public StartGameMessage(string sessionKey) : base(MessageType.StartGame, sessionKey)
        {
        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - Game Started {DateTime.Now}");
        }

    }
}
