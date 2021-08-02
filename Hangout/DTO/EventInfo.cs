using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.DTO
{
    public class EventInfo
    {
        public EventInfo(int eventId, string eventName, short cityId, string road, DateTime hostTime, DateTime deadline, 
            int eventPrice, string cover, string eventContent, float costTime, short personLimit, short typeId, int memberId,
            byte status, IEnumerable<int> messageIds, IEnumerable<int> peopleIds, IEnumerable<DateTime> times, IEnumerable<string> messageContents,
            IEnumerable<int> participantIds, IEnumerable<int> participantGroups, IEnumerable<string> motivations, double? lat, double? lng)
        {
            EventId = eventId;
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
            MemberId = memberId;
            Status = status;
            MessageIds = messageIds;
            PeopleIds = peopleIds;
            Times = times;
            MessageContents = messageContents;
            ParticipantIds = participantIds;
            ParticipantGroups = participantGroups;
            Motivations = motivations;
            Lat = lat;
            Lng = lng;
        }

        public int EventId { get; set; }
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
        public int MemberId { get; set; }
        public byte Status { get; set; }
   
        public IEnumerable<int> MessageIds { get; set; }
        public IEnumerable<int> PeopleIds { get; set; }
        public IEnumerable<DateTime> Times { get; set; }
        public IEnumerable<string> MessageContents { get; set; }

        public IEnumerable<int> ParticipantIds { get; set; }
        public IEnumerable<int> ParticipantGroups { get; set; }
        public IEnumerable<string> Motivations { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }

    }
}
