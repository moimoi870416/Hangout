using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Relationship
    {
        public Relationship(int protagonistId, int objectId)
        {
            ProtagonistId = protagonistId;
            ObjectId = objectId;
        }

        public int RelationshipId { get; set; }
        public int ProtagonistId { get; set; }
        public int ObjectId { get; set; }
    }
}
