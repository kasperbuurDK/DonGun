﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedClassLibrary.MessageStrings.Message;

namespace SharedClassLibrary.MessageStrings
{
    public class StartGameMessage: Message
    {
        public StartGameMessage(string sessionKey) : base(MessageType.StartGame, sessionKey)
        {

        }
        
        
        public override string ToString()
        {
            return string.Format($"{base.ToString()} - Game Started {DateTime.Now}");
        }

    }
}
