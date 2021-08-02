using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.Output
{
    public class EventOutput
    {
        public EventOutput(int eventId, int memberId, string eventName, short cityId, string road, 
            DateTime hostTime, DateTime deadline, int eventPrice, string cover, string eventContent, 
            float costTime, short personLimit, short typeId, bool status, double? lat, double? lng)
        {
            EventId = eventId;
            MemberId = memberId;
            EventName = eventName;
            CityId = cityId;
            Road = road;
            HostTime = hostTime;
            Deadline = deadline;
            EventPrice = eventPrice;
            Cover = cover;
            EventContent = eventContent;
            CostTime = costTime;
            PersonLimit = personLimit;
            TypeId = typeId;
            Status = status;
            Lat = lat;
            Lng = lng;
        }

        public int EventId { get; set; }

        public int MemberId { get; set; }
        public string EventName { get; set; }
        public short CityId { get; set; }
        public string Road { get; set; }
        public DateTime HostTime { get; set; }
        public DateTime Deadline { get; set; }
        public int EventPrice { get; set; }
       
        public string Cover { get; set; }
        public string EventContent { get; set; }
        public float CostTime { get; set; }
        public short PersonLimit { get; set; }
        public short TypeId { get; set; }
        public bool Status { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }

    }
}
