using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Event
    {
        public Event(string eventName, int addressId, DateTime hostTime, DateTime deadline, int eventPrice, string cover,
            string eventContent, float costTime, short personLimit, short typeId, int memberId, double? lat, double? lng)
        {
            EventName = eventName;
            AddressId = addressId;
            HostTime = hostTime;
            Deadline = deadline;
            EventPrice = eventPrice;
            Cover = cover;
            EventContent = eventContent;
            CostTime = costTime;
            PersonLimit = personLimit;
            TypeId = typeId;
            MemberId = memberId;
            Lat = lat;
            Lng = lng;
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public int AddressId { get; set; }
        public DateTime HostTime { get; set; }
        public DateTime Deadline { get; set; }
        public int EventPrice { get; set; }
        public string Cover { get; set; }
        public string EventContent { get; set; }
        public float CostTime { get; set; }
        public short PersonLimit { get; set; }
        public short TypeId { get; set; }
        public byte Status { get; set; }
        public int MemberId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
