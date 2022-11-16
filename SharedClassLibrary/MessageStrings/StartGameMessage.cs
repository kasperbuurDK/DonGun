using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedClassLibrary.MessageStrings.Message;

namespace SharedClassLibrary.MessageStrings
{
    public class StartGameMessage: Message
    {
        public string CupContents { get; set; } = string.Empty;

        public Queue<Character> theQueue { get; set; }
        
        public StartGameMessage(string sessionKey) : base(MessageType.StartGame, sessionKey)
        {

        }
        
        
        public override string ToString()
        {
            return string.Format($"{base.ToString()} - {CupContents}");
        }

    }
}
