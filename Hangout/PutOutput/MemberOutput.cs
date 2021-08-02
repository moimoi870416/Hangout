using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Output
{
    public class MemberOutput
    {
        public string? Password { get; set; }
        public string Name { get; set; }
        //public IFormFile MemberPhoto { get; set; }
        public byte? Gender { get; set; }

        public DateTime? Birth { get; set; }
        public short? CityId { get; set; }
        public string Category { get; set; }
        public string JobTitle { get; set; }
        public string Intro { get; set; }
        public List<int> Type { get; set; }
    }
}
