using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Comment
    {
        public Comment(int objectId, int memberId, double rate, string commentContent)
        {
            ObjectId = objectId;
            MemberId = memberId;
            Rate = rate;
            CommentContent = commentContent;
        }

        public int CommentId { get; set; }
        public int ObjectId { get; set; }
        public int MemberId { get; set; }
        public double Rate { get; set; }
        public string CommentContent { get; set; }
        public DateTime Time { get; set; }
    }
}
