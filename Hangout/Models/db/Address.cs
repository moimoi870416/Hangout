using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public short CityId { get; set; }
        public string Road { get; set; }
    }
}
