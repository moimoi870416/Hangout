using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Parameter
{
    public class MemberInput
    {

        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
     
        public byte Gender { get; set; }

        public List<IFormFile> MemberPhoto { get; set; }
        public DateTime Birth { get; set; }
        public short CityId { get; set; }
        public string Category { get; set; }
        public string JobTitle { get; set; }
        public string Intro { get; set; }
        public List<int> Type{get; set;}
    }
}
