using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public class D20 : Dice
    {
        public readonly string Prefix;
        public D20(string prefix = "dtwenty", int timeSpan = 1000)
            : base(20, 1) 
        {
            RollingDuration = new TimeSpan(0, 0, 0, 0, timeSpan);
            Prefix = prefix;
        }

        public string ImageName()
        {
            return String.Format($"{Prefix}_d{(NumAsAlpha)Result}roll.png");
        }

        public enum NumAsAlpha
        {
            a = 1,
            b,
            c,
            d,
            e,
            f,
            g,
            h,
            i,
            j,
            k,
            l,
            m,
            n,
            o,
            p,
            q,
            r,
            s,
            t
        }
    }
}