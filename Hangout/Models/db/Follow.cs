using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Follow
    {
        public Follow(int protagonistId, int objectId)
        {
            ProtagonistId = protagonistId;
            ObjectId = objectId;
        }

        public int FollowId { get; set; }
        public int ProtagonistId { get; set; }
        public int ObjectId { get; set; }
    }
}
