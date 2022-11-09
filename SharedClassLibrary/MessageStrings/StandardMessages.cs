using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.MessageStrings
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
