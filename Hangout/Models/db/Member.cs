using System;
using System.Collections.Generic;

#nullable disable

namespace Hangout.Models.db
{
    public partial class Member
    {
        public Member(string account, string password, string name, string gender, DateTime birth, short cityId, string category, string jobTitle, string intro)
        {
            Account = account;
            Password = password;
            Name = name;
            Gender = gender;
            Birth = birth;
            CityId = cityId;
            Category = category;
            JobTitle = jobTitle;
            Intro = intro;
        }

        public int MemberId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Birth { get; set; }
        public short CityId { get; set; }
        public string Category { get; set; }
        public string JobTitle { get; set; }
        public string Intro { get; set; }
    }
}
