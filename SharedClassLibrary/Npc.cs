using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public class Npc : Character_abstract
    {
        public Npc()
        {
            Race = new Elf();
        }
        
    }
}
