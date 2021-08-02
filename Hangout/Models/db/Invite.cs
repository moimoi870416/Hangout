using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Invite
    {
        public Invite(int protagonistId, int objectId, int eventId)
        {
            ProtagonistId = protagonistId;
            ObjectId = objectId;
            EventId = eventId;
        }

        public int InviteId { get; set; }
        public int ProtagonistId { get; set; }
        public int ObjectId { get; set; }
        public int EventId { get; set; }
    }
}
