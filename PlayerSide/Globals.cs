
using SharedClassLibrary;

namespace PlayerSide
{
    public static class Globals
    {
        public static Character_abstract Connectivity { get; set;}
        public static List<Character_abstract> GameOrder { get; set; }
        public static RestService<Player> RSPlayerInfo { get; set; }
        public static RestService<Npc> RSNonPlayerInfo { get; set; }
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
