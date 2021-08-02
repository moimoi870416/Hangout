using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Parameter
{
    public class ImageInput
    {

        public IFormFile Imageo { get; set; }

        internal Task CopyToAsync(FileStream fileStream)
        {
            throw new NotImplementedException();
        }
    }
}
