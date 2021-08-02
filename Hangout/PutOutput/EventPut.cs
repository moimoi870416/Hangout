using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.PutOutput
{
    public class EventPut
    {
        public string EventName { get; set; }
        public short? CityId { get; set; }
        public string Road { get; set; }
        public DateTime? HostTime { get; set; }
        public DateTime? Deadline { get; set; }
        public int? EventPrice { get; set; }
        
        public IFormFile? Cover { get; set; }
        public string EventContent { get; set; }
        public float? CostTime { get; set; }
        public short? PersonLimit { get; set; }
        public short? TypeId { get; set; }
        public bool? Status { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
