using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Notice
    {
        public Notice(int protagonistId, string content, int objectId, int eventId)
        {
            ProtagonistId = protagonistId;
            Content = content;
            ObjectId = objectId;
            EventId = eventId;
        }

        public int NoticeId { get; set; }
        public int ProtagonistId { get; set; }
        public string Content { get; set; }
        public int ObjectId { get; set; }
        public int EventId { get; set; }
        public byte Status { get; set; }
    }
}
