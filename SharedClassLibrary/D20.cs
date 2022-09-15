using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public class D20 : Dice
    {
        public D20()
            : base(20, 1) 
        {
            RollingDuration = new TimeSpan(0, 0, 0, 0, 1000);
        }
    }
}
