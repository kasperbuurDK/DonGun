using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public class GameSessionOptions : MessageStrings.Message
    {
        public Player Sheet { get; set; } = new Player(); 
    }
}
