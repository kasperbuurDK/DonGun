using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.MessageStrings
{
    public class DiceRolledMessage : Message
    {
        public string UserName { get; set; } = string.Empty;
        public string CupContents { get; set; } = string.Empty;
        public string LastModified { get; set; } = string.Empty;

        public DiceRolledMessage() : base(MessageType.DiceEvent) { }

        public DiceRolledMessage(string SKey) : base(MessageType.DiceEvent, SKey) { }

        public DiceRolledMessage(Cup cup) : base(MessageType.DiceEvent) 
        {
            CupContents = cup.ToString();
        }

        public override string ToString()
        {
            return string.Format($"{UserName} - {CupContents} - {LastModified}");
        }
    }
}
