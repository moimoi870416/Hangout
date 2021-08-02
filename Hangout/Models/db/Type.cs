using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Type
    {
        public int TypeId { get; set; }
        public int MemberId { get; set; }
        public int TypeNum { get; set; }
    }
}
