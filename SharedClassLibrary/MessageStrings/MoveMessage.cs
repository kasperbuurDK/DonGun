using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.MessageStrings
{
    public class MoveMessage : Message
    {
        public string Direction { get; set; } = string.Empty;
        public int Distence { get; set; }

        public MoveMessage() : base(MessageType.MoveEvent) { }

        public MoveMessage(string SKey) : base(MessageType.MoveEvent, SKey) { }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {Direction} - {Distence}");
        }
    }
}
