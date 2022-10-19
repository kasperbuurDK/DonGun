
using SharedClassLibrary;

namespace PlayerSide
{
    public static class Globals
    {
        public static Character_abstract Connectivity { get; set; }
        public static List<Character_abstract> GameOrder { get; set; }

        public static RestService<List<User>, User> RestUserInfo { get; set; }

        public static HubService<FileUpdateMessage> FileUpdateHub { get; set; }

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
