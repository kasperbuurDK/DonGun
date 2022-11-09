﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Actions
{
    internal class InspireAlly: HelperAction
    {
        public InspireAlly(string senderSig, string recieverSig) : base(senderSig, recieverSig)
        {
        }

        public override bool MakeBasicAction(int diceValue, Character sender, Character reciever)
        {
            reciever.RecieveInspiration(sender.CalculateInspiration());
            return true;
        }
    }
}


