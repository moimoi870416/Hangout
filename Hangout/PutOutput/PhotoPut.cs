using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.PutOutput
{
    public class PhotoPut
    {
        public List<IFormFile> MemberPhoto { get; set; }
    }
}
