using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Actions
{
    public abstract class HelperAction : IAnAction
    {
        public Character Reciever { get; set; }
        public int ChanceToSucced { get; set; }
        public Character Sender { get; set; }
        public abstract string Signature { get;}
        public abstract bool MakeBasicAction(int diceValue);
       
    }
}
