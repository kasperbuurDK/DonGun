using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public class FileUpdateMessage
    {
        public string UserName { get; set; }
        public string SheetId { get; set; }
        public string SessionKey { get; set; }
        public string LastModified { get; set; }
    }
}
