using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Message
    {
        public Message(int eventId, int memberId, string messageContent)
        {
            EventId = eventId;
            MemberId = memberId;
            MessageContent = messageContent;
        }

        public Message(int eventId, int memberId, string messageContent, byte status) : this(eventId, memberId, messageContent)
        {
            Status = status;
        }

        public int MessageId { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public DateTime Time { get; set; }
        public string MessageContent { get; set; }
        public byte Status { get; set; }
    }
}
