using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Actions
{
    internal class HealAlly : HelperAction
    {
        public override string Signature { get => "Heal:" + Reciever.Name; }
        public override bool MakeBasicAction(int diceValue)
        {
            Reciever.RecieveHealing(Sender.CalculateHealing());
            return true;
        }

        
    }
}
