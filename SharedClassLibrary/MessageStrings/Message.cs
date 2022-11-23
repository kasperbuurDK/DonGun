using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.MessageStrings
{
    public class Message
    {
        public string UserName { get; set; } = string.Empty;
        public MessageType MsgType { get; set; }
        public string SessionKey { get; set; }
        public string? ConnectionId { get; set; }
        public string LastModified { get; set; } = string.Empty;

        public Message(MessageType msgType) 
        {
            MsgType = msgType;
            SessionKey ??= string.Empty;
        }
        public Message(MessageType msgType, string sessionKey) : this(msgType)
        {
            SessionKey = sessionKey;
        }
        public Message() : this(MessageType.StdEvent, string.Empty) { }

        public enum MessageType
        {
            ErrorEvent,
            FileEvent,
            ActionEvent,
            StdEvent,
            MoveEvent,
            UpdateEvent,
            StartGame,
            NewTurn,
            EndMyTurn
        }

        public override string ToString()
        {
            return string.Format($"{UserName} - {LastModified} - {SessionKey}");
        }
    }
}
