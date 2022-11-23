namespace SharedClassLibrary.MessageStrings
{
    public class EndMyTurnMessage : Message
    {

        public EndMyTurnMessage(string sessionKey) : base(MessageType.EndMyTurn, sessionKey)
        {

        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {UserName}'s Turn Ended {DateTime.Now}");
        }

    }
}







