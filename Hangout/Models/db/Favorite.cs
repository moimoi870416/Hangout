using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Favorite
    {
        public Favorite(int memberId, int eventNum)
        {
            MemberId = memberId;
            EventNum = eventNum;
        }

        public int FavoritesId { get; set; }
        public int MemberId { get; set; }
        public int EventNum { get; set; }
    }
}
