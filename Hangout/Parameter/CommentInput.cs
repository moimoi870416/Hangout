using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Parameter
{
    public class CommentInput
    {
        public int ObjectId { get; set; }
        public int MemberId { get; set; }
        public double Rate { get; set; }
        public string CommentContent { get; set; }
    }
}
