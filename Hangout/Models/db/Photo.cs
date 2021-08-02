using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Photo
    {
        public int PhotoId { get; set; }
        public int MemberId { get; set; }
        public string Path { get; set; }
    }
}
