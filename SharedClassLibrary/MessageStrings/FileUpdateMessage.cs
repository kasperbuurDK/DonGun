using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.MessageStrings
{
    public class FileUpdateMessage : Message
    {
        public string UUID { get; set; } = string.Empty;
        public string SheetId { get; set; } = string.Empty;

        public FileUpdateMessage() : base(MessageType.FileEvent) { }

        public FileUpdateMessage(string SKey) : base(MessageType.FileEvent, SKey) { }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {UUID} - {SheetId}");
        }
    }
}
