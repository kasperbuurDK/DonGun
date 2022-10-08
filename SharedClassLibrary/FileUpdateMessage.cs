using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public class FileUpdateMessage
    {
        public string UserName { get; set; } = string.Empty;
        public string UUID { get; set; } = string.Empty;
        public string SheetId { get; set; } = string.Empty;
        public string LastModified { get; set; } = string.Empty;

        public override string ToString()
        {
            return string.Format($"{UserName} - {UUID} - {SheetId} - {LastModified}");
        }
    }
}
