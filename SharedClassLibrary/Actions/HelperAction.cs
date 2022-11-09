using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Actions
{
    public abstract class HelperAction : IAnAction
    {
      //  public Character Reciever { get; set; }
        public int ChanceToSucced { get; set; }
      // public Character Sender { get; set; }
        public string Signature { get; init; }
        public string SenderSignature { get ; set ; }
        public string RecieverSignature { get; set; }

        public HelperAction(string senderSig, string recieverSig)
        {
            Signature = Guid.NewGuid().ToString();
            SenderSignature = senderSig;
            RecieverSignature = recieverSig;
        }

        public abstract bool MakeBasicAction(int diceValue, Character sender, Character reciever);

        
    }
}
