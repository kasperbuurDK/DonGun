
using SharedClassLibrary;

namespace PlayerSide
{
    public static class Globals
    {
        public static Character_abstract Connectivity;
        public static event EventHandler ConnectivityChanged;
        public static RestService<Npc> RService;

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
