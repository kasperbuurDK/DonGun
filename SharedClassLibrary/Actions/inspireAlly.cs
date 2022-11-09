using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Actions
{
    internal class InspireAlly: HelperAction
    {
        public override string Signature => throw new NotImplementedException();

        public override bool MakeBasicAction(int diceValue)
        {
            base.Reciever.RecieveInspiration(base.Sender.CalculateInspiration());
            return true;
        }

        
    }
}


