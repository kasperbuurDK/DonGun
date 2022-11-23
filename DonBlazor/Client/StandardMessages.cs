using SharedClassLibrary.MessageStrings;

namespace DonBlazor.Client
{
    public class StandardMessages : Message
    {
        public static string AllOK => "OK";

        public static string NotEnough(string ressource, string action)
        {
            return $"Sorry, you do not have enough {ressource} to perform the {action}";
        }
    }
}
