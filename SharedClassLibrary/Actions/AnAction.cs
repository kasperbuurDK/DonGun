using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.Actions
{
    public class AnAction
    {
        public string SenderSignature { get; set; } = string.Empty;
        public string RecieverSignature { get; set; } = string.Empty;
        public int ChanceToSucced { get; set; }
        public string Signature { get; init; } = string.Empty;
    }
}
