using SharedClassLibrary.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.MessageStrings
{   
    public class UpdateMessage : Message
    {

        public string UpdateStr { get; set; } = string.Empty;

        public UpdateMessage() : base(MessageType.UpdateEvent) { }

        public UpdateMessage(string SKey) : base(MessageType.UpdateEvent, SKey) { }

        public List<IAnAction> possibleActions;

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {UpdateStr}");
        }
    }
}
