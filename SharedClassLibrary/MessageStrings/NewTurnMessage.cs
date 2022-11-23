using SharedClassLibrary.MessageStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedClassLibrary.MessageStrings.Message;

namespace SharedClassLibrary.MessageStrings
{
    public class NewTurnMessage: Message
    {
        public Queue<Character> TheQueue { get; set; }
        public List<string> Happenings { get; set; }


        public NewTurnMessage(string sessionKey, Queue<Character> queue, List<string> happenings) : base(MessageType.NewTurn, sessionKey)
        {
            TheQueue = queue;
            Happenings = happenings;
        }

        public override string ToString()
        {
            return string.Format($"{base.ToString()} - New Turn Started {DateTime.Now}");
        }

    }
}


