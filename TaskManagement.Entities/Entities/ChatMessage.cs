using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Entities
{
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
        public string Recipient { get; set; }
    }
}
