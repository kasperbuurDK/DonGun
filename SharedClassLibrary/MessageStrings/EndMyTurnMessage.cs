using SharedClassLibrary.MessageStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedClassLibrary.MessageStrings.Message;

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






  
