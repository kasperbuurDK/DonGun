
namespace SharedClassLibrary.MessageStrings
{
    public class ExceptionMessage : Message
    {
        public string Exception { get; set; } = string.Empty;

        public ExceptionMessage(string SKey, string e) : base(MessageType.ExceptionEvent, SKey)
        {
            Exception = e;
        }
        public ExceptionMessage() : base(MessageType.ExceptionEvent)
        {
        }


        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {Exception}");
        }
    }
}
