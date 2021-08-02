using Hangout.DTO;
using Hangout.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangout
{
    public class EventCenter
    {
        public HangoutContext HangoutContext { get; } //連接資料庫內容
        public EventCenter(HangoutContext hangoutContext)
        {
            HangoutContext = hangoutContext;
        }
        /// <summary>
        /// 查找指定日期前所有的活動
        /// </summary>
        /// <param name="date">篩選時間</param>
        /// <returns></returns>
        public IEnumerable<Event> GetEventByDate(DateTime? date)
        {
            return HangoutContext.Events.Where(data => data.Deadline >= date);
        }

        /// <summary>
        /// 查找指定活動編號相同之活動
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <returns></returns>
        public Event GetEvent(int EventId)
        {
            return HangoutContext.Events.Where(data => data.EventId == EventId).SingleOrDefault();
        }

        /// <summary>
        ///  查找指定活動編號與會員編號相同之活動
        /// </summary>
        /// <param name="EventId">活動編號</param>
        /// <param name="MemberId">會員編號</param>
        /// <returns></returns>
        public Event GetEventByMember(int EventId,int MemberId)
        {
            return HangoutContext.Events.Where(data => data.EventId == EventId && data.MemberId==MemberId ).SingleOrDefault();
        }

        /// <summary>
        /// 輸入地址編號 獲得內容
        /// </summary>
        /// <param name="AdressId"></param>
        /// <returns></returns>
        public Address GetAdress(int AdressId)
        {
            return HangoutContext.Addresses.Where(data => data.AddressId == AdressId).SingleOrDefault();
        }

        public Event GetEventCheck(int MemberId , DateTime HoldTime)
        {
           return HangoutContext.Events.Where(data => data.MemberId == MemberId && data.HostTime == HoldTime).SingleOrDefault();
        }

    }
}
