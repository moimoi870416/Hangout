using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Parameter
{
    public class MessageInput
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public string MessageContent { get; set; }
        public byte Status { get; set; }

    }
}
