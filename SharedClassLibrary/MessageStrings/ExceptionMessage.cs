
namespace SharedClassLibrary.MessageStrings
{
    public class ExceptionMessage : Message
    {
        public string Exception;

        public ExceptionMessage(string SKey, string e) : base(MessageType.ExceptionEvent, SKey)
        {
            Exception = e;
        }


        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {Exception}");
        }
    }
}
