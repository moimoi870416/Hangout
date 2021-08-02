using Hangout.Models.db;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout.DTO
{
    public class MemberInfo
    {
        public MemberInfo(int memberId, string account, string password, string name, IEnumerable<string> memberPhoto, byte gender,
            DateTime birth, short cityId, string category, string jobTitle, string intro, IEnumerable<int> types, 
            IEnumerable<int> commentIds, IEnumerable<int> peloplerIds, IEnumerable<double> rates, IEnumerable<string> commentContents, 
            IEnumerable<DateTime> times , IEnumerable<int> holdEvent, IEnumerable<int> joinEvent, IEnumerable<int> inviteEvent)
        {
            MemberId = memberId;
            Account = account;
            Password = password;
            Name = name;
            MemberPhoto = memberPhoto;
            Gender = gender;
            Birth = birth;
            CityId = cityId;
            Category = category;
            JobTitle = jobTitle;
            Intro = intro;
            Types = types;
            CommentIds = commentIds;
            PeloplerIds = peloplerIds;
            Rates = rates;
            CommentContents = commentContents;
            Times = times;
            HoldEvent = holdEvent;
            JoinEvent=joinEvent;
            InviteEvent = inviteEvent;
        }

        public int MemberId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> MemberPhoto { get; set; }
        public byte Gender { get; set; }
        public DateTime Birth { get; set; }
        public short CityId { get; set; }
        public string Category { get; set; }
        public string JobTitle { get; set; }
        public string Intro { get; set; }
        public IEnumerable<int> Types { get; set; }
        public IEnumerable<int> CommentIds { get; set; }
        public IEnumerable<int> PeloplerIds { get; set; }
        public IEnumerable<double> Rates { get; set; }
        public IEnumerable<string> CommentContents { get; set; }
        public IEnumerable<DateTime> Times { get; set; }

        public IEnumerable<int> HoldEvent { get; set; }
        public IEnumerable<int> JoinEvent { get; set; }
        public IEnumerable<int> InviteEvent { get; set; }


    }
}
